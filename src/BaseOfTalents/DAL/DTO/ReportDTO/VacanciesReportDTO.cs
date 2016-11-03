using DAL.DTO.ReportDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class VacanciesReportDTO: VacancyReportDTO
    {
        public int VacanciesPendingInCurrentPeriodCount { get; set; }
        public int VacanciesOpenedInCurrentPeriodCount { get; set; }
        public int VacanciesInProgressInCurrentPeriodCount { get; set; }
        public int VacanciesClosedInCurrentPeriodCount { get; set; }
        public int VacanciesCanceledInCurrentPeriodCount { get; set; }
    }
}
