using Domain.Entities.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EFData.Mapping
{
    public class DepartmentConfiguration : BaseEntityConfiguration<Department>
    {
        public DepartmentConfiguration()
        {
            Property(d => d.Title).IsRequired();
        }
    }
}
