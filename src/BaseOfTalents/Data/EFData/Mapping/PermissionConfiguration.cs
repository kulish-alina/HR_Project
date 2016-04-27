using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EFData.Mapping
{
    public class PermissionConfiguration : BaseEntityConfiguration<Permission>
    {
        public PermissionConfiguration()
        {
            Property(p => p.Description).IsRequired();
            Property(p => p.AccessRights).IsRequired();
        }
    }
}
