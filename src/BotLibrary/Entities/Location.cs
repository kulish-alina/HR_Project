using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotLibrary.Entities
{
    public class Location: BaseEntity
    {
        public City City { get; set; }
    }
}
