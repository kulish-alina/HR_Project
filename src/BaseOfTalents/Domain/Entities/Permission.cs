using Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Permission : BaseEntity
    {
        public Permission()
        {
            Roles = new List<Role>();
        }

        public string Description { get; set; }
        public AccessRights AccessRights { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
    }
}
