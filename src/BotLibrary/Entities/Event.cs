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
        public EventType EventType {get; set;}
        public DateTime EventDate { get; set; }
        public string Description { get; set; }
        public Vacancy Vacancy { get; set; }
        public Candidate Candidate { get; set; }
        public User Responsible { get; set; }
        public Room Room { get; set; }
    }
}
