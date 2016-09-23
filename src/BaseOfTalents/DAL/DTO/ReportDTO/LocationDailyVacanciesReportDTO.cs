using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.ReportDTO
{
    public class LocationDailyVacanciesReportDTO
    {
        public LocationDailyVacanciesReportDTO()
        {
            DailyVacanciesStatisticsInfo = new List<DailyVacanciesReportDTO>();
        }
        public int LocationId { get; set; }
        public List<DailyVacanciesReportDTO> DailyVacanciesStatisticsInfo { get; set; }
    }
}
