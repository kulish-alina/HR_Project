using BotLibrary.Entities.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotLibrary.Entities
{
    public class Event: BaseEntity
    {
        DateTime EventDate { get; set; }
        string Description { get; set; }
        Vacancy Vacancy { get; set; }
        Candidate Candidate { get; set; }
        User Responsible { get; set; }
        Room Room { get; set; }
    }
}
