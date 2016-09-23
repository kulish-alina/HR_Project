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
        public string UserDisplayName { get; set; }

        // key - stage id (added candidates have key 0 because it does not belong to the stages),
        // value - count of candidates on this stage
        public Dictionary<int, int> StagesData { get; set; } 
    }
}
