using BotLibrary.Entities.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotLibrary.Entities
{
    public class StageInfo: BaseEntity
    {
        Stage Stage { get; set; }
        Comment Comment { get; set; }
    }
}
