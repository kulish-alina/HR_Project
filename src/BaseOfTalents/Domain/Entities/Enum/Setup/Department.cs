using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Setup
{
    public class Department : BaseEntity
    {
        public string Title { get; set; }

        public int DepartmentGroupId { get; set; }
        public DepartmentGroup DepartmentGroup { get; set; }
    }
}

