using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotDomain.Entities
{
    public class Role: BaseEntity
    {
        public string Name { get; set; }
        public int PermissionId { get; set; }
    }
}
