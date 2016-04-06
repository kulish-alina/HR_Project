using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotLibrary.Entities
{
    public class Permission : BaseEntity
    {
        public Role Role { get; set; }
        public string Description { get; set; }
    }
}
