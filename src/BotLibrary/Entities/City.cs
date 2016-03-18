using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotLibrary.Entities
{
    public class City: BaseEntity
    {
        public Country Country { get; set; }
        public string Name { get; set; }
    }
}
