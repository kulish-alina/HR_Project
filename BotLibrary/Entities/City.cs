using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotLibrary.Entities
{
    public class City: BaseEntity
    {
        Country Country { get; set; }
        string Name { get; set; }
    }
}
