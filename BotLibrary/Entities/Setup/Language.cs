using BotLibrary.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotLibrary.Entities.Setup
{
    public class Language: BaseEntity
    {
        string Name { get; set; }
        LanguageLevel LanguageLevel { get; set; }
    }
}
