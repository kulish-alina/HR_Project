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
        public DateTime EventDate { get; set; }
        public string Description { get; set; }

        public int EventTypeId { get; set; }
        public virtual EventType EventType { get; set; }

        public virtual Vacancy Vacancy { get; set; }
        public virtual Candidate Candidate { get; set; }

        public int ResponsibleId { get; set; }
        public virtual User Responsible { get; set; }

    }
}
