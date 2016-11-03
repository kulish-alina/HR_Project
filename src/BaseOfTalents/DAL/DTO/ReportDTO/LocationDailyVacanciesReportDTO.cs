using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.ReportDTO
{
    public class LocationDailyVacanciesReportDTO: LocationVacancyReportDTO
    {
        public LocationDailyVacanciesReportDTO()
        {
            VacanciesStatisticsInfo = new List<DailyVacanciesReportDTO>();
        }
        public List<DailyVacanciesReportDTO> VacanciesStatisticsInfo { get; set; }
    }
}
