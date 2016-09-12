using DAL.DTO.ReportDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class UsersReportDTO: BaseEntityDTO
    {
        public UsersReportDTO()
        {
            StatisticsInfo = new List<StatisticsDTO>();
        }
        public int UserId { get; set; }
        public List<StatisticsDTO> StatisticsInfo { get; set; }
    }
}
