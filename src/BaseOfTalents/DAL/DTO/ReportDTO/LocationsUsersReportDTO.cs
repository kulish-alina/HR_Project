using System.Collections.Generic;

namespace DAL.DTO.ReportDTO
{
    public class LocationsUsersReportDTO
    {
        public LocationsUsersReportDTO()
        {
            UsersStatisticsInfo = new List<UsersReportDTO>();
        }
        public int? LocationId { get; set; }
        public List<UsersReportDTO> UsersStatisticsInfo { get; set; }
    }
}
