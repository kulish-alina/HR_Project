using DAL.DTO.ReportDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class UsersReportDTO
    {
        public UsersReportDTO()
        {
            StagesData = new Dictionary<int, int>();
        }
        public int UserId { get; set; }
        public string DisplayName { get; set; }

        public Dictionary<int, int> StagesData { get; set; } //key stage Id (added = id-0), vslue - data from db
    }
}
