using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.ReportDTO
{
    public class LocationsVacanciesReportDTO
    {
        public LocationsVacanciesReportDTO()
        {
            VacanciesStatisticsInfo = new List<VacanciesReportDTO>();
        }
        public int LocationId { get; set; }
        public List<VacanciesReportDTO> VacanciesStatisticsInfo { get; set; }
    }
}
