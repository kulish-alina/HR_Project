using Domain.Entities.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Event: BaseEntity
    {
        public virtual EventType EventType { get; set; }
        public DateTime EventDate { get; set; }
        public string Description { get; set; }
        public virtual Vacancy Vacancy { get; set; }
        public virtual Candidate Candidate { get; set; }
        public virtual User Responsible { get; set; }
        public virtual Room Room { get; set; }
    }
}
