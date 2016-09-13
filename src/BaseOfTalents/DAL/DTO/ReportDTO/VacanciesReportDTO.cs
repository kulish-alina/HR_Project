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
        public string DisplayName { get; set; }
        public int VacanciesOpenedInPreviousPeriodCount { get; set; }
        public int VacanciesOpenedInCurrentPeriodCount { get; set; }
        public int VacanciesInProgress { get; set; }
        public int VacanciesClosed { get; set; }
    }
}
