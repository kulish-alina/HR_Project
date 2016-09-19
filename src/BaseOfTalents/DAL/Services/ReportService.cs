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

        public IEnumerable<LocationsVacanciesReportDTO> GetVacanciesReportData(
            ICollection<int> locationIds,
            ICollection<int> userIds,
            DateTime startDate,
            DateTime endDate)
        {
            var statesCreatedInCurrentPeriodFilter = new List<Expression<Func<VacancyState, bool>>>();
            statesCreatedInCurrentPeriodFilter.Add(x => x.CreatedOn > startDate && x.CreatedOn < endDate);
            statesCreatedInCurrentPeriodFilter.Add(x => !userIds.Any() || userIds.Contains(x.Vacancy.ResponsibleId));
            statesCreatedInCurrentPeriodFilter.Add(x => !locationIds.Any() || x.Vacancy.Cities.Any(y => locationIds.Contains(y.Id)));

            var statesCreatedInCurrentPeriod = uow.VacancyStateRepo.Get(statesCreatedInCurrentPeriodFilter);

            var vacanciesGroupedByLocations = statesCreatedInCurrentPeriod.Select(x => x.Vacancy).GroupBy(x => x.Cities.First().Id);

            var result = new List<LocationsVacanciesReportDTO>();

            foreach (var vacanciesGroup in vacanciesGroupedByLocations)
            {
                var locationReport = new LocationsVacanciesReportDTO() { LocationId = vacanciesGroup.Key };
                foreach (var vacancy in vacanciesGroup)
                {
                    var usersReport = new VacanciesReportDTO() {
                        UserId = vacancy.ResponsibleId,
                        DisplayName = string.Format("{0} {1}", vacancy.Responsible.LastName, vacancy.Responsible.FirstName),
                        VacanciesPendingInCurrentPeriodCount = GetVacanciesCountForUser(statesCreatedInCurrentPeriod, vacancy.ResponsibleId, EntityState.Pending),
                        VacanciesOpenedInCurrentPeriodCount = GetVacanciesCountForUser(statesCreatedInCurrentPeriod, vacancy.ResponsibleId, EntityState.Open),
                        VacanciesInProgressInCurrentPeriodCount = GetVacanciesCountForUser(statesCreatedInCurrentPeriod, vacancy.ResponsibleId, EntityState.Processing),
                        VacanciesClosedInCurrentPeriodCount = GetVacanciesCountForUser(statesCreatedInCurrentPeriod, vacancy.ResponsibleId, EntityState.Closed),
                        VacanciesClosedInCanceledPeriodCount = GetVacanciesCountForUser(statesCreatedInCurrentPeriod, vacancy.ResponsibleId, EntityState.Cancelled)
                    };
                    locationReport.VacanciesStatisticsInfo.Add(usersReport);
                }
                result.Add(locationReport); 
            }

            return result;
        }

        private int GetVacanciesCountForUser(IEnumerable<VacancyState> states, int responsibleId, EntityState state)
        {
            return states.Where(x => x.State == state).Select(x => x.Vacancy).Where(x => x.ResponsibleId == responsibleId).Count();
        }

        public IEnumerable<LocationDailyVacanciesReportDTO> GetDailyVacanciesReportData(
            ICollection<int> locationIds,
            ICollection<int> userIds,
            DateTime date)
        {
            var statesFilter = new List<Expression<Func<VacancyState, bool>>>();
            statesFilter.Add(x => x.CreatedOn > date);
            statesFilter.Add(x => x.Passed == null);
            statesFilter.Add(x => x.State == EntityState.Pending || x.State == EntityState.Open || x.State == EntityState.Pending);
            statesFilter.Add(x => !userIds.Any() || userIds.Contains(x.Vacancy.ResponsibleId));
            statesFilter.Add(x => !locationIds.Any() || x.Vacancy.Cities.Any(y => locationIds.Contains(y.Id)));

            var states = uow.VacancyStateRepo.Get(statesFilter);

            var vacanciesGroupedByLocations = states.Select(x => x.Vacancy).GroupBy(x => x.Cities.First().Id);

            var result = new List<LocationDailyVacanciesReportDTO>();

            foreach (var vacanciesGroup in vacanciesGroupedByLocations)
            {
                var locationReport = new LocationDailyVacanciesReportDTO() { LocationId = vacanciesGroup.Key };
                foreach (var vacancy in vacanciesGroup)
                {
                    var usersReport = new DailyVacanciesReportDTO()
                    {
                        UserId = vacancy.ResponsibleId,
                        DisplayName = string.Format("{0} {1}", vacancy.Responsible.LastName, vacancy.Responsible.FirstName),
                        PendinVacanciesCount = GetVacanciesCountForUser(states, vacancy.ResponsibleId, EntityState.Pending),
                        OpenVacanciesCount = GetVacanciesCountForUser(states, vacancy.ResponsibleId, EntityState.Open),
                        InProgressVacanciesCount = GetVacanciesCountForUser(states, vacancy.ResponsibleId, EntityState.Processing)
                    };
                    locationReport.DailyVacanciesStatisticsInfo.Add(usersReport);
                }
                result.Add(locationReport);
            }
            return result;
        }
    }
    }
}
