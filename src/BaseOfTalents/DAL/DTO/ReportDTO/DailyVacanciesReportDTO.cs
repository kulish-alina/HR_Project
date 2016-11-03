using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.ReportDTO
{
    public class DailyVacanciesReportDTO : VacancyReportDTO
    {
        public int PendingVacanciesCount { get; set; }
        public int OpenVacanciesCount { get; set; }
        public int InProgressVacanciesCount { get; set; }
    }
}
