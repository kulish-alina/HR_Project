using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Setup
{
    public class DepartmentGroup : BaseEntity
    {
        public string Title { get; set; }
        public virtual ICollection<Department> Departments { get; set; }
    }
}
