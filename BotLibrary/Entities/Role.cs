using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotLibrary.Entities
{
    public class Role: BaseEntity
    {
        string Name { get; set; }
        int PermissionId { get; set; }
    }
}
