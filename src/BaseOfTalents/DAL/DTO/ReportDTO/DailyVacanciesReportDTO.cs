using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.ReportDTO
{
    public class DailyVacanciesReportDTO : BaseEntityDTO
    {
        public int UserId { get; set; }
        public string DisplayName { get; set; }
        public int PendinVacanciesCount { get; set; }
        public int OpenVacanciesCount { get; set; }
        public int InProgressVacanciesCount { get; set; }
    }
}
