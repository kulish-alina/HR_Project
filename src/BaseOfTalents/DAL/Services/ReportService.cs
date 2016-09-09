using DAL.DTO;
using DAL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public object GetUsersReportData(ICollection<int> locationIds, object userIds, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public object GetVacanciesReportData(ICollection<int> locationIds, object userIds, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }
    }
}
