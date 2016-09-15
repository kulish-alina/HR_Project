using DAL.DTO;
using DAL.DTO.ReportDTO;
using DAL.Infrastructure;
using Domain.Entities;
using Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
{
    public class ReportService
    {
        IUnitOfWork uow;
        public ReportService(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public IEnumerable<LocationsUsersReportDTO> GetUsersReportData(
            ICollection<int> locationIds,
            ICollection<int> userIds,
            DateTime startDate,
            DateTime endDate)
        {
            var stagesFilter = new List<Expression<Func<VacancyStageInfo, bool>>>();
            stagesFilter.Add(x => x.CreatedOn > startDate && x.CreatedOn < endDate);

            var stages = uow.VacancyStageInfoRepo.Get(stagesFilter);

            var usersGroup = stages.Select(x => x.Vacancy).Select(x => x.Responsible)
                .Where(x => userIds.Count() == 0 || userIds.Contains(x.Id))
                .Where(x => locationIds.Count() == 0 || locationIds.Contains(x.City.Id))
                .GroupBy(x => x.CityId);

            var result = new List<LocationsUsersReportDTO>();

            foreach (var usersInCity in usersGroup)
            {
                var current = new LocationsUsersReportDTO() { LocationId = usersInCity.Key };                
                foreach (var user in usersInCity)
                {
                    var userReport = new UsersReportDTO()
                    {
                        UserId = user.Id,
                        DisplayName = string.Format("{0} {1}", user.LastName, user.FirstName),
                        StagesData = GetStagesDataForUser(stages, user.Id)
                    };
                    userReport.StagesData.Add(0, GetAddedCandedatesCount(startDate, endDate, user.Id));

                    current.UsersStatisticsInfo.Add(userReport);
                }
                result.Add(current);
            }
            
            return result;
        }

        private int GetAddedCandedatesCount(DateTime startDate, DateTime endDate, int userId)
        {
            var candidatesFilter = new List<Expression<Func<Candidate, bool>>>();
            candidatesFilter.Add(x => x.CreatedOn > startDate && x.CreatedOn < endDate);

            var candidates = uow.CandidateRepo.Get(candidatesFilter);
            return candidates.Where(x => x.CreatorId == userId).Count();
        }

        private Dictionary<int, int> GetStagesDataForUser(IEnumerable<VacancyStageInfo> stages, int userId)
        {
            var result = new Dictionary<int, int>();
            var data = stages.Where(s => s.Vacancy.ResponsibleId == userId).GroupBy(x => x.StageId).Select(s => new { StageId = s.Key, Data = s.Count() });
            foreach (var item in data)
            {
                result.Add(item.StageId, item.Data);
            }
            return result;
        }

        public IEnumerable<LocationsUsersReportDTO> GetVacanciesReportData(
            ICollection<int> locationIds,
            ICollection<int> userIds,
            DateTime startDate,
            DateTime endDate)
        {
            //var vacanciesOfPreviousPeriodStateFilrer = new List<Expression<Func<VacancyState, bool>>>();
            //vacanciesOfPreviousPeriodStateFilrer.Add(x => (x.State == EntityState.Open ||
            //x.State == EntityState.Processing) && (x.CreatedOn < startDate));
            //var stagesFromPreviousPeriod = uow.VacancyStateRepo.Get(vacanciesOfPreviousPeriodStateFilrer);

            //var vacanciesOfPreviousPeriod = GetFilterVacancies(stagesFromPreviousPeriod, locationIds, userIds);

            //var vacanciesOpenedInCurrentPeriodStateFilrer = new List<Expression<Func<VacancyState, bool>>>();
            //vacanciesOpenedInCurrentPeriodStateFilrer.Add(x => x.State == EntityState.Open  && 
            //(x.CreatedOn > startDate && x.CreatedOn < endDate));
            //var stagesOpenInCurrentPeriod = uow.VacancyStateRepo.Get(vacanciesOpenedInCurrentPeriodStateFilrer);

            //var vacanciesOpenedInCurrentPeriod = GetFilterVacancies(stagesOpenInCurrentPeriod, locationIds, userIds);


            throw new NotImplementedException();
        }

        //private IEnumerable<Vacancy> GetFilterVacancies(IEnumerable<VacancyState> states, IEnumerable<int> locationIds, IEnumerable<int> userIds)
        //{
        //    return states.Select(x => x.Vacancy)
        //        .Where(x => !userIds.Any() || userIds.Contains(x.ResponsibleId))
        //        .Where(x => !locationIds.Any() || x.Cities.Any(y => locationIds.Contains(y.Id)));
        //}
    }
}
