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
               .ToLookup(x => x.VacancyId)
               .Select(x => new CandidateProgressReportUnitDTO
               {
                   CandidateId = candidate.Id,
                   CandidateFirstName = candidate.FirstName,
                   CandidateLastName = candidate.LastName,
                   VacancyId = x.Key,
                   VacancyTitle = x.First().Vacancy.Title,
                   Stages = x.Select(vsi => new StageInfoDTO
                   {
                       StageId = vsi.StageId,
                       Comment = vsi.Comment?.Message,
                       PassDate = vsi.DateOfPass.Value,
                       StageTitle = vsi.Stage.Title
                   })
               });
        }

        public static IEnumerable<T> GetByLocations<T>(this IEnumerable<T> source, IEnumerable<int> locationsIds) where T : VacancyStageInfo
        {
            return source.Where(x => locationsIds.Any(location => x.Vacancy.Cities.Any(city => city.Id == location))).ToList();
        }
    }
}
