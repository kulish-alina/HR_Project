using BotLibrary.Entities.Enum;
using BotLibrary.Entities.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotLibrary.Entities
{
    public class WorkInfo: BaseEntity
    {
        string PositionDesired { get; set; }
        int SalaryDesired { get; set; }
        List<Skill> Skills { get; set; }
        LanguageLevel TypeOfEmployment { get; set; }
        string Practice { get; set; }
        Experience Experience { get; set; }
    }
}
