using DAL.DTO;
using DAL.DTO.ReportDTO;
using DAL.Infrastructure;
using Domain.Entities;
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

            var usersGroup = stages.Select(x => x.Vacancy).Select(x => x.Responsible).GroupBy(x => x.CityId);

            var result = new List<LocationsUsersReportDTO>();

            foreach (var usersInCity in usersGroup)
            {
                var current = new LocationsUsersReportDTO() { LocationId = usersInCity.Key };                
                foreach (var user in usersInCity)
                {
                    var userReportDto = new UsersReportDTO()
                    {
                        UserId = user.Id,
                        DisplayName = string.Format("{0} {1}", user.LastName, user.FirstName),
                        StagesData = GetStagesDataForUser(stages, user.Id)
                    };
                    userReportDto.StagesData.Add(0, GetAddedCatesCount(stages, user.Id));

                    current.UsersStatisticsInfo.Add(userReportDto);
                }
                result.Add(current);
            }
            
            return result;
        }

        private int GetAddedCatesCount(IEnumerable<VacancyStageInfo> stages, int p)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}
