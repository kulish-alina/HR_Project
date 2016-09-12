using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.ReportDTO
{
    public class LocationsUsersReportDTO
    {
        public LocationsUsersReportDTO()
        {
            UsersStatisticsInfo = new List<UsersReportDTO>();
        }
        public int LocationId { get; set; }
        public List<UsersReportDTO> UsersStatisticsInfo { get; set; }
    }
}
