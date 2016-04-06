using BotLibrary.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotLibrary.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime EditTime { get; set; }
        public EntityState State { get; set; }
    }
}
