using BotDomain.Entities.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotDomain.Entities
{
    public class StageInfo: BaseEntity
    {
        public Stage Stage { get; set; }
        public Comment Comment { get; set; }
    }
}
