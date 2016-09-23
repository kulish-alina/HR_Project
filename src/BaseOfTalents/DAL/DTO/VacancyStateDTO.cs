using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class VacancyStateDTO : BaseEntityDTO
    {
        public int VacancyId { get; set; }
        public DateTime? Passed { get; set; }
    }
}
