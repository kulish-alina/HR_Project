using DAL.DTO;
using DAL.Infrastructure;
using Domain.Entities;
using Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace DAL.LoggerCore
{
    public class Logger
    {
        private static Func<KeyValuePair<int, VacancyStageInfo>, bool> isIdToVsiExists = (destinationVsi) => !destinationVsi.Equals(default(KeyValuePair<int, VacancyStageInfo>));
        private static Func<KeyValuePair<int, VacancyStageInfo>, KeyValuePair<int, VacancyStageInfoDTO>, bool> StagesAreEquals =
            (destinationVsi, vacancyIdToActiveStage) => destinationVsi.Value.StageId != vacancyIdToActiveStage.Value.StageId;
        private static Func<string, bool> isCitiesOrLevels = (field) => Regex.IsMatch(field, "(citi|level)", RegexOptions.IgnoreCase);
        private readonly static String EMPTY = "*empty*";

        /// <summary> 
        /// Performs a comparison and logs, if appropriate field changed
        /// </summary>
        /// <typeparam name="T">Entity that implement ILogable interface</typeparam>
        /// <typeparam name="S">Approproate to destination dto object</typeparam>
        /// <param name="destination">Serverside entity</param>
        /// <param name="source">Entity from clientside, that probably changed</param>
        /// <param name="userId">User id, who perfoms the action</param>
        public static void Log<T, S>(T destination, S source, int userId, IUnitOfWork uow)
            where T : ILogable
            where S : BaseEntityDTO
        {
            var destinationPropertiesAndValue = getPropsAndValueOf(typeof(T), destination);
            var sourcePropertiesAndValue = getPropsAndValueOf(typeof(S), source);

            foreach (var destinationKvp in destinationPropertiesAndValue)
            {
                var fieldToLog = LogConfig.FieldsToLog.FirstOrDefault(x => Regex.IsMatch(destinationKvp.Key, x.DomainFieldName, RegexOptions.IgnoreCase));
                var sourceKvp = sourcePropertiesAndValue.FirstOrDefault(x => x.Key.Contains(fieldToLog.DTOFieldName));
                if (destinationKvp.Key.Contains("Progress"))
                {
                    HandleVacanciesProgressCase(destination, userId, destinationKvp, sourceKvp, uow);
                }
                else if (isCitiesOrLevels(destinationKvp.Key))
                {
                    var sourceIds = sourceKvp.Value as List<int>;
                    var destinationIds = new List<int>();
                    if (destinationKvp.Value != null)
                    {
                        destinationIds = (((IEnumerable<BaseEntity>)destinationKvp.Value)).Select(x => x.Id).ToList();
                    }
                    if (!sourceIds.SequenceEqual(destinationIds))
                    {
                        destination.Log(new LogUnit
                        {
                            Field = destinationKvp.Key,
                            UserId = userId,
                            NewValues = CreateLogValueListOf(sourceIds.Select(x => x.ToString()).ToArray<string>()),
                            PastValues = CreateLogValueListOf(destinationIds.Select(x => x.ToString()).ToArray<string>()),
                            FieldType = FieldType.Array
                        });
                    }
                }
                else if (!Object.Equals(destinationKvp.Value, sourceKvp.Value))
                {
                    var sourceValue = sourceKvp.Value;
                    var destinationValue = destinationKvp.Value;
                    if (Regex.IsMatch(destinationKvp.Key, @"description", RegexOptions.IgnoreCase))
                    {
                        sourceValue = StripHTML(sourceKvp.Value != null ? sourceKvp.Value.ToString() : EMPTY);
                        destinationValue = StripHTML(destinationKvp.Value != null ? destinationKvp.Value.ToString() : EMPTY);
                    }
                    destination.Log(new LogUnit
                    {
                        Field = destinationKvp.Key,
                        UserId = userId,
                        NewValues = CreateLogValueListOf(sourceValue != null ? sourceValue.ToString() : EMPTY),
                        PastValues = CreateLogValueListOf(destinationValue != null ? destinationValue.ToString() : EMPTY),
                        FieldType = FieldType.Plain
                    });
                }
            }
        }

        public static string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }

        private static ICollection<LogValue> CreateLogValueListOf(params string[] value)
        {
            return value.Select(x => new LogValue { Value = x }).ToList();
        }

        //TODO: handle delete case
        private static void HandleVacanciesProgressCase<T>(
            T destination,
            int userId, KeyValuePair<string, object> destinationFieldAndValue,
            KeyValuePair<string, object> sourceFieldAndValue,
            IUnitOfWork uow)
            where T : ILogable
        {
            var destinationVSIs = destinationFieldAndValue.Value as List<VacancyStageInfo>;
            var sourceVSIs = sourceFieldAndValue.Value as List<VacancyStageInfoDTO>;
            var isCandidate = typeof(T).Name == "Candidate";
            var fieldType = isCandidate ? FieldType.VacanciesProgress : FieldType.CandidatesProgress;
            if (sourceVSIs == null || !sourceVSIs.Any())
            {
                return;
            }

            var destGrouped = isCandidate ? GroupDestinationVSIsByVacancyId(destinationVSIs) : GroupDestinationVSIsByCandidateId(destinationVSIs);
            var sourceGrouped = isCandidate ? GroupSourceVSIsByVacancyId(sourceVSIs) : GroupSourceVSIsByCandidateId(sourceVSIs);

            var destinationIdsToActiveStages = GetDestinationActiveStage(destGrouped);
            var sourceIdsToActiveStages = GetSourceActiveStage(sourceGrouped);

            foreach (var sourceIdToActiveStage in sourceIdsToActiveStages)
            {
                var destinationIdToActiveStage = GetAppropriateDestinationVsi(destinationIdsToActiveStages, sourceIdToActiveStage);
                var isNewAttchedEntity = destGrouped.FirstOrDefault(x => x.Key == sourceIdToActiveStage.Key).Value == null;
                if (sourceIdToActiveStage.Value.Id == 0 && isNewAttchedEntity)
                {
                    destination.Log(new LogUnit
                    {
                        UserId = userId,
                        Field = $"{sourceIdToActiveStage.Key}",
                        NewValues = CreateLogValueListOf($"{sourceIdToActiveStage.Value.StageId}"),
                        PastValues = CreateLogValueListOf($"{EMPTY}"),
                        FieldType = fieldType
                    });
                    if (isCandidate)
                    {
                        uow.VacancyRepo.GetByID(sourceIdToActiveStage.Key).Log(new LogUnit
                        {
                            UserId = userId,
                            Field = $"{sourceIdToActiveStage.Value.CandidateId}",
                            NewValues = CreateLogValueListOf($"{sourceIdToActiveStage.Value.StageId}"),
                            PastValues = CreateLogValueListOf($"{EMPTY}"),
                            FieldType = FieldType.CandidatesProgress
                        });
                    }
                    else
                    {
                        uow.CandidateRepo.GetByID(sourceIdToActiveStage.Key).Log(new LogUnit
                        {
                            UserId = userId,
                            Field = $"{sourceIdToActiveStage.Value.VacancyId}",
                            NewValues = CreateLogValueListOf($"{sourceIdToActiveStage.Value.StageId}"),
                            PastValues = CreateLogValueListOf($"{EMPTY}"),
                            FieldType = FieldType.VacanciesProgress
                        });
                    }
                }
                else if (isIdToVsiExists(destinationIdToActiveStage)
                  && StagesAreEquals(destinationIdToActiveStage, sourceIdToActiveStage))
                {
                    destination.Log(new LogUnit
                    {
                        UserId = userId,
                        Field = $"{sourceIdToActiveStage.Key}",
                        NewValues = CreateLogValueListOf($"{sourceIdToActiveStage.Value.StageId}"),
                        PastValues = CreateLogValueListOf($"{destinationIdToActiveStage.Value.StageId}"),
                        FieldType = fieldType
                    });
                    if (isCandidate)
                    {
                        uow.VacancyRepo.GetByID(sourceIdToActiveStage.Key).Log(new LogUnit
                        {
                            UserId = userId,
                            Field = $"{sourceIdToActiveStage.Value.CandidateId}",
                            NewValues = CreateLogValueListOf($"{sourceIdToActiveStage.Value.StageId}"),
                            PastValues = CreateLogValueListOf($"{destinationIdToActiveStage.Value.StageId}"),
                            FieldType = FieldType.CandidatesProgress
                        });
                    }
                    else
                    {
                        uow.CandidateRepo.GetByID(sourceIdToActiveStage.Key).Log(new LogUnit
                        {
                            UserId = userId,
                            Field = $"{sourceIdToActiveStage.Value.VacancyId}",
                            NewValues = CreateLogValueListOf($"{sourceIdToActiveStage.Value.StageId}"),
                            PastValues = CreateLogValueListOf($"{destinationIdToActiveStage.Value.StageId}"),
                            FieldType = FieldType.VacanciesProgress
                        });
                    }
                }
            }
        }

        private static Dictionary<int, List<VacancyStageInfoDTO>> GroupSourceVSIsByCandidateId(List<VacancyStageInfoDTO> sourceVSIs)
        {
            return sourceVSIs.Aggregate(new Dictionary<int, List<VacancyStageInfoDTO>>(), (acc, vsi) =>
            {
                if (acc.ContainsKey(vsi.CandidateId))
                {
                    acc[vsi.CandidateId].Add(vsi);
                }
                else
                {
                    acc.Add(vsi.CandidateId, new List<VacancyStageInfoDTO>
                    {
                        vsi
                    });
                }
                return acc;
            });
        }

        private static Dictionary<int, List<VacancyStageInfo>> GroupDestinationVSIsByCandidateId(List<VacancyStageInfo> destinationVSIs)
        {
            return destinationVSIs.Aggregate(new Dictionary<int, List<VacancyStageInfo>>(), (acc, vsi) =>
            {
                if (acc.ContainsKey(vsi.CandidateId))
                {
                    acc[vsi.CandidateId].Add(vsi);
                }
                else
                {
                    acc.Add(vsi.CandidateId, new List<VacancyStageInfo>
                    {
                        vsi
                    });
                }
                return acc;
            });
        }

        private static Dictionary<int, List<VacancyStageInfo>> GroupDestinationVSIsByVacancyId(List<VacancyStageInfo> destinationVSIs)
        {
            return destinationVSIs.Aggregate(new Dictionary<int, List<VacancyStageInfo>>(), (acc, vsi) =>
            {
                if (acc.ContainsKey(vsi.VacancyId))
                {
                    acc[vsi.VacancyId].Add(vsi);
                }
                else
                {
                    acc.Add(vsi.VacancyId, new List<VacancyStageInfo>
                    {
                        vsi
                    });
                }
                return acc;
            });
        }

        private static KeyValuePair<int, VacancyStageInfo> GetAppropriateDestinationVsi(Dictionary<int, VacancyStageInfo> destinationVacancyIdsToActiveStages, KeyValuePair<int, VacancyStageInfoDTO> vacancyIdToActiveStage)
        {
            return destinationVacancyIdsToActiveStages.FirstOrDefault(x => x.Key == vacancyIdToActiveStage.Key);
        }

        private static Dictionary<int, VacancyStageInfoDTO> GetSourceActiveStage(Dictionary<int, List<VacancyStageInfoDTO>> sourceGroupedVSIs)
        {
            return sourceGroupedVSIs
                    .ToDictionary(x => x.Key,
                              y => y.Value
                                .FirstOrDefault(x => x.StageState == StageState.Active));
        }

        private static Dictionary<int, VacancyStageInfo> GetDestinationActiveStage(Dictionary<int, List<VacancyStageInfo>> destinationGroupedVSIs)
        {
            return destinationGroupedVSIs
                    .ToDictionary(x => x.Key,
                              y => y.Value
                                .FirstOrDefault(x => x.StageState == StageState.Active));
        }

        private static Dictionary<int, List<VacancyStageInfoDTO>> GroupSourceVSIsByVacancyId(List<VacancyStageInfoDTO> sourceVSIs)
        {
            return sourceVSIs.Aggregate(new Dictionary<int, List<VacancyStageInfoDTO>>(), (acc, vsi) =>
            {
                if (acc.ContainsKey(vsi.VacancyId))
                {
                    acc[vsi.VacancyId].Add(vsi);
                }
                else
                {
                    acc.Add(vsi.VacancyId, new List<VacancyStageInfoDTO>
                    {
                        vsi
                    });
                }
                return acc;
            });
        }



        private static Dictionary<string, object> getPropsAndValueOf<T>(Type type, T concreteObject)
        {
            return getPropertiesOf(type).ToDictionary(x => x.Name, y => y.GetValue(concreteObject));
        }

        private static IEnumerable<PropertyInfo> getPropertiesOf(Type type)
        {
            return from fieldInfo in type.GetProperties()
                   where LogConfig.FieldsToLog.Any(x =>
                                  (fieldInfo.Name.Contains(x.DomainFieldName) || fieldInfo.Name.Contains(x.DTOFieldName))
                               && !fieldInfo.Name.Contains("StatesInfo"))
                   select fieldInfo;
        }
    }
}
