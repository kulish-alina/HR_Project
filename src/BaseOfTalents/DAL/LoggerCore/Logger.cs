using DAL.DTO;
using Domain.Entities;
using Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DAL.LoggerCore
{
    public class Logger
    {
        private static Func<KeyValuePair<int, VacancyStageInfo>, bool> isVacancyIdToVsiExists = (destinationVsi) => !destinationVsi.Equals(default(KeyValuePair<int, VacancyStageInfo>));
        private static Func<KeyValuePair<int, VacancyStageInfo>, KeyValuePair<int, VacancyStageInfoDTO>, bool> StagesAreEquals =
            (destinationVsi, vacancyIdToActiveStage) => destinationVsi.Value.StageId != vacancyIdToActiveStage.Value.StageId;

        /// <summary> 
        /// Performs a comparison and logs, if appropriate field changed
        /// </summary>
        /// <typeparam name="T">Entity that implement ILogable interface</typeparam>
        /// <typeparam name="S">Approproate to destination dto object</typeparam>
        /// <param name="destination">Serverside entity</param>
        /// <param name="source">Entity from clientside, that probably changed</param>
        /// <param name="userId">User id, who perfoms the action</param>
        public static void Log<T, S>(T destination, S source, int userId)
            where T : ILogable
            where S : BaseEntityDTO
        {
            var destinationPropertiesAndValue = getPropsAndValueOf(typeof(T), destination);
            var sourcePropertiesAndValue = getPropsAndValueOf(typeof(S), source);

            foreach (var destinationKvp in destinationPropertiesAndValue)
            {
                var sourceKvp = sourcePropertiesAndValue.FirstOrDefault(x => x.Key.Equals(destinationKvp.Key));
                if (destinationKvp.Key.Equals("VacanciesProgress") && typeof(T).Name == "Candidate")
                {

                    HandleVacanciesProgressCase(destination, userId, destinationKvp, sourceKvp);
                }
                else if (!destinationKvp.Value.Equals(sourceKvp.Value))
                {
                    destination.Log(new LogUnit
                    {
                        Field = destinationKvp.Key,
                        UserId = userId,
                        Value = sourceKvp.Value.ToString()
                    });
                }
            }
        }

        //TODO: handle delete case
        private static void HandleVacanciesProgressCase<T>(
            T destination,
            int userId, KeyValuePair<string, object> destinationFieldAndValue,
            KeyValuePair<string, object> sourceFieldAndValue)
            where T : ILogable
        {
            var destinationVSIs = destinationFieldAndValue.Value as List<VacancyStageInfo>;
            var sourceVSIs = sourceFieldAndValue.Value as List<VacancyStageInfoDTO>;
            if (sourceVSIs == null || !sourceVSIs.Any())
            {
                return;
            }

            var destinationVacancyIdsToActiveStages = GetDestinationActiveStage(GroupDestinationVSIsByVacancyId(destinationVSIs));
            var sourceVacancyIdsToActiveStages = GetSourceActiveStage(GroupSourceVSIsByVacancyId(sourceVSIs));

            foreach (var sourceVacancyIdToActiveStage in sourceVacancyIdsToActiveStages)
            {
                var destinationVacancyIdToActiveStage = GetAppropriateDestinationVsi(destinationVacancyIdsToActiveStages, sourceVacancyIdToActiveStage);
                if (isVacancyIdToVsiExists(destinationVacancyIdToActiveStage)
                    && StagesAreEquals(destinationVacancyIdToActiveStage, sourceVacancyIdToActiveStage))
                {
                    destination.Log(new LogUnit
                    {
                        UserId = userId,
                        Field = $"Vacancy {sourceVacancyIdToActiveStage.Key}",
                        Value = $"Stage {sourceVacancyIdToActiveStage.Value.StageId}"
                    });
                }
            }
        }

        private static KeyValuePair<int, VacancyStageInfo> GetAppropriateDestinationVsi(Dictionary<int, VacancyStageInfo> destinationVacancyIdsToActiveStages, KeyValuePair<int, VacancyStageInfoDTO> vacancyIdToActiveStage)
        {
            return destinationVacancyIdsToActiveStages.FirstOrDefault(x => x.Key == vacancyIdToActiveStage.Key);
        }

        private static Dictionary<int, VacancyStageInfoDTO> GetSourceActiveStage(Dictionary<int, List<VacancyStageInfoDTO>> sourceVsisGroupedByVacancyId)
        {
            return sourceVsisGroupedByVacancyId
                    .ToDictionary(x => x.Key,
                              y => y.Value
                                .FirstOrDefault(x => x.StageState == StageState.Active));
        }

        private static Dictionary<int, VacancyStageInfo> GetDestinationActiveStage(Dictionary<int, List<VacancyStageInfo>> destVsisGroupedByVacancyId)
        {
            return destVsisGroupedByVacancyId
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

        private static Dictionary<string, object> getPropsAndValueOf<T>(Type type, T concreteObject)
        {
            return getPropertiesOf(type).ToDictionary(x => x.Name, y => y.GetValue(concreteObject));
        }

        private static IEnumerable<PropertyInfo> getPropertiesOf(Type type)
        {
            return from fieldInfo in type.GetProperties()
                   where LogConfig.FieldToLog.Any(x => fieldInfo.Name.Contains(x))
                   select fieldInfo;
        }
    }
}
