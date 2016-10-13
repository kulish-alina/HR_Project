using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class VacancyState : BaseEntity
    {
        public int VacancyId { get; set; }
        public virtual Vacancy Vacancy { get; set; }
        public DateTime? Passed { get; set; }
    }
}
