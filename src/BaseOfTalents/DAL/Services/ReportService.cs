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
            var usersFilter = new List<Expression<Func<Vacancy, bool>>>();

            if (userIds.Count != 0 )
            {
                usersFilter.Add(x => userIds.Contains(x.Responsible.Id));
            }
            var vacancies = uow.VacancyRepo.Get(usersFilter);
            var groupedVacancies = vacancies.GroupBy(x => x.Responsible);
            foreach (var group in groupedVacancies)
            {
                var groupKey = group.Key;
                IEnumerable<IGrouping<int, VacancyStageInfo>> groupedVacancyStageInfo = null;
                foreach (var vacancy in group)
                {
                    if(groupedVacancyStageInfo == null)
                    {
                        groupedVacancyStageInfo = vacancy.CandidatesProgress.Where(y => y.StageState == Domain.Entities.Enum.StageState.Active).GroupBy(y => y.StageId);
                    } else
                    {
                        var rez = vacancy.CandidatesProgress.Where(y => y.StageState == Domain.Entities.Enum.StageState.Active).GroupBy(y => y.StageId);
                        groupedVacancyStageInfo.Concat(rez);
                    }
                    
                }      
            }
            vacancies.ToList().ForEach(x =>
            {
                var groupedVacancyStageInfo = x.CandidatesProgress.Where(y => y.StageState == Domain.Entities.Enum.StageState.Active).GroupBy(y => y.StageId);

            });
            return new List<LocationsUsersReportDTO>();
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
