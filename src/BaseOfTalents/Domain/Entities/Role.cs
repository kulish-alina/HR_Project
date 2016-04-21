using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Entities
{
    public class Role : BaseEntity
    {
        public string Title { get; set; }
        public virtual List<Permission> Permissions { get; set; }
    }
}
