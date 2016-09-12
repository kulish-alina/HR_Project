using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.ReportDTO
{
    public class StatisticsDTO: BaseEntityDTO
    {
        public string StatisticalIndicatorName { get; set; }
        public int StatisticalIndicatorValue { get; set; }
    }
}
