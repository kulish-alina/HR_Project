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
            // select all stages created  for the selected period
            var stagesFilter = new List<Expression<Func<VacancyStageInfo, bool>>>()
            {
                x => x.CreatedOn > startDate && x.CreatedOn < endDate
            };

            var stages = uow.VacancyStageInfoRepo.Get(stagesFilter);

            // select responsible users from vacancies filtered by userIds and locationIds and grouped by location
            var usersGroup = stages.Select(x => x.Vacancy.Responsible)
                .Where(x => !userIds.Any() || userIds.Contains(x.Id))
                .Where(x => !locationIds.Any() || locationIds.Contains(x.City.Id))
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
                        UserDisplayName = $"{user.LastName} {user.FirstName}",
                        StagesData = GetStagesDataForUser(stages, user.Id)
                    };
                    // data for added candidates
                    userReport.StagesData.Add(0, GetAddedCandedatesCount(startDate, endDate, user.Id));

                    current.UsersStatisticsInfo.Add(userReport);
                }
                result.Add(current);
            }
            
            return result;
        }

        private int GetAddedCandedatesCount(DateTime startDate, DateTime endDate, int userId)
        {
            // select all candidates created  for the selected period
            var candidatesFilter = new List<Expression<Func<Candidate, bool>>>()
            {
                x => x.CreatedOn > startDate && x.CreatedOn < endDate
            };
            var candidates = uow.CandidateRepo.Get(candidatesFilter);

            // calculate all candidated added in a data base by some user
            return candidates.Count(x => x.CreatorId == userId);
        }

        private Dictionary<int, int> GetStagesDataForUser(IEnumerable<VacancyStageInfo> stages, int userId)
        {
            var data = stages
                .Where(s => s.Vacancy.ResponsibleId == userId)
                .GroupBy(x => x.StageId)
                .Select(s => new { StageId = s.Key, Data = s.Count() })
                .ToDictionary(key => key.StageId, val => val.Data);
            return data;
        }

        public IEnumerable<LocationsVacanciesReportDTO> GetVacanciesReportData(
            ICollection<int> locationIds,
            ICollection<int> userIds,
            DateTime startDate,
            DateTime? endDate)
        {
            // select vacancies' states for the selected period filtered by userIds and locationIds
            var statesCreatedInCurrentPeriod = GetFilteredVacanciesStatesForVacanciesReport(locationIds, userIds, startDate, endDate);

            var vacanciesGroupedByLocations = statesCreatedInCurrentPeriod
                .Select(x => x.Vacancy)
                .GroupBy(x => x.Cities.First().Id);

            var result = vacanciesGroupedByLocations.Select(x => new LocationsVacanciesReportDTO
            {
                LocationId = x.Key,
                VacanciesStatisticsInfo = x.Select(y => CreateVacanciesReport(
                    y.ResponsibleId,
                    y.Responsible.FirstName,
                    y.Responsible.LastName,
                    statesCreatedInCurrentPeriod)).ToList()
            });

            return result;
        }


        private VacanciesReportDTO CreateVacanciesReport(int responsibleId,
            string responsibleFirstName, string responsibleLastName,
            IEnumerable<VacancyState> vacancyStates)
        {
            return new VacanciesReportDTO
            {
                UserId = responsibleId,
                UserDisplayName = $"{responsibleFirstName} {responsibleLastName}",
                VacanciesPendingInCurrentPeriodCount = GetVacanciesCountForUser(vacancyStates, responsibleId, EntityState.Pending),
                VacanciesOpenedInCurrentPeriodCount = GetVacanciesCountForUser(vacancyStates, responsibleId, EntityState.Open),
                VacanciesInProgressInCurrentPeriodCount = GetVacanciesCountForUser(vacancyStates, responsibleId, EntityState.Processing),
                VacanciesClosedInCurrentPeriodCount = GetVacanciesCountForUser(vacancyStates, responsibleId, EntityState.Closed),
                VacanciesClosedInCanceledPeriodCount = GetVacanciesCountForUser(vacancyStates, responsibleId, EntityState.Cancelled)
            };
        }

        public IEnumerable<LocationDailyVacanciesReportDTO> GetDailyVacanciesReportData(
            ICollection<int> locationIds,
            ICollection<int> userIds,
            DateTime? date)
        {
            var states = GetFilteredVacanciesStatesForDailyVacanciesReport(locationIds, userIds, date);

            var vacanciesGroupedByLocations = states.Select(x => x.Vacancy)
                .GroupBy(x => x.Cities.First().Id);

            var result = vacanciesGroupedByLocations.Select(x => new LocationDailyVacanciesReportDTO
            {
                LocationId = x.Key,
                DailyVacanciesStatisticsInfo = x.Select(y => CreateDailyVacanciesReport(y.ResponsibleId, y.Responsible.FirstName, y.Responsible.LastName, states)).ToList()
            });

            return result;
        }


        private IEnumerable<VacancyState> GetFilteredVacanciesStatesForVacanciesReport(
            ICollection<int> locationIds,
            ICollection<int> userIds,
            DateTime startDate,
            DateTime? endDate)
        {
            var statesCreatedInCurrentPeriodFilter = new List<Expression<Func<VacancyState, bool>>>() {
                x => x.CreatedOn > startDate && x.CreatedOn < endDate,
                x => !userIds.Any() || userIds.Contains(x.Vacancy.ResponsibleId),
                x => !locationIds.Any() || x.Vacancy.Cities.Any(y => locationIds.Contains(y.Id))
            };

            return uow.VacancyStateRepo.Get(statesCreatedInCurrentPeriodFilter);
        }

        private IEnumerable<VacancyState> GetFilteredVacanciesStatesForDailyVacanciesReport(
            ICollection<int> locationIds,
            ICollection<int> userIds,
            DateTime? date)
        {
            var statesFilter = new List<Expression<Func<VacancyState, bool>>>() {
                x => x.Passed < date || x.Passed == null,
                x => x.State == EntityState.Pending || x.State == EntityState.Open || x.State == EntityState.Processing,
                x => !userIds.Any() || userIds.Contains(x.Vacancy.ResponsibleId),
                x => !locationIds.Any() || x.Vacancy.Cities.Any(y => locationIds.Contains(y.Id))
            };

            var filteredStates = uow.VacancyStateRepo.Get(statesFilter);

            return filteredStates.Where(x => x.CreatedOn.Value.Date <= date);
        }

        private DailyVacanciesReportDTO CreateDailyVacanciesReport(int responsibleId,
            string responsibleFirstName, string responsibleLastName,
            IEnumerable<VacancyState> states)
        {
            return new DailyVacanciesReportDTO
            {
                UserId = responsibleId,
                UserDisplayName = $"{responsibleFirstName} {responsibleLastName}",
                PendingVacanciesCount = GetVacanciesCountForUser(states, responsibleId, EntityState.Pending),
                OpenVacanciesCount = GetVacanciesCountForUser(states, responsibleId, EntityState.Open),
                InProgressVacanciesCount = GetVacanciesCountForUser(states, responsibleId, EntityState.Processing)
            };
        }

        private int GetVacanciesCountForUser(IEnumerable<VacancyState> states, int responsibleId, EntityState state)
        {
            return states.Where(x => x.State == state).Select(x => x.Vacancy).Where(x => x.ResponsibleId == responsibleId).Count();
        }
    }
 }

