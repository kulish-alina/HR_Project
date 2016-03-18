using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotDomain.Entities
{
    public class Location: BaseEntity
    {
        public City City { get; set; }
    }
}
