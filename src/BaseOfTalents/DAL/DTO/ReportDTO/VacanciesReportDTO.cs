using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class VacanciesReportDTO: BaseEntityDTO
    {
        public int UserId { get; set; }
        public string UserDisplayName { get; set; }
        public int VacanciesPendingInCurrentPeriodCount { get; set; }
        public int VacanciesOpenedInCurrentPeriodCount { get; set; }
        public int VacanciesInProgressInCurrentPeriodCount { get; set; }
        public int VacanciesClosedInCurrentPeriodCount { get; set; }
        public int VacanciesClosedInCanceledPeriodCount { get; set; }
    }
}
