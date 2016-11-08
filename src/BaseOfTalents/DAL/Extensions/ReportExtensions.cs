using DAL.DTO.ReportDTO;
using Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Extensions
{
    public static class ReportExtensions
    {
        public static IEnumerable<CandidateProgressReportUnitDTO> GetReport<T>(this IEnumerable<T> source, Candidate candidate) where T : VacancyStageInfo
        {
            return source
               .GroupByVacancyId()
               .Select(x => new CandidateProgressReportUnitDTO
               {
                   CandidateId = candidate.Id,
                   CandidateFirstName = candidate.FirstName,
                   CandidateLastName = candidate.LastName,
                   VacancyId = x.Key,
                   VacancyTitle = x.Value.First().Vacancy.Title,
                   Stages = x.Value.Select(vsi => new StageInfoDTO
                   {
                       StageId = vsi.StageId,
                       Comment = vsi.Comment?.Message,
                       PassDate = vsi.DateOfPass.Value,
                       StageTitle = vsi.Stage.Title
                   })
               });
        }
        private static Dictionary<int, List<T>> GroupByVacancyId<T>(this IEnumerable<T> source) where T : VacancyStageInfo
        {
            return source.Aggregate(new Dictionary<int, List<T>>(), (group, vsi) =>
            {
                if (group.Any(pair => pair.Key == vsi.VacancyId))
                {
                    group[vsi.VacancyId].Add(vsi);
                }
                else
                {
                    group.Add(vsi.VacancyId, new List<T> { vsi });
                }
                return group;
            });
        }
    }
}
