using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Entities
{
    public class Role : BaseEntity
    {
        public Role()
        {
            Permissions = new List<Permission>();
        }
        public string Title { get; set; }
        public virtual ICollection<Permission> Permissions { get; set; }
    }
}
