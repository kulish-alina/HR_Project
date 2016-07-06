using Domain.Entities.Enum.Setup;
using System;

namespace Domain.Entities
{
    public class Event : BaseEntity
    {
        public DateTime EventDate { get; set; }
        public string Description { get; set; }
        public int ResponsibleId { get; set; }
        public virtual User Responsible { get; set; }

        public int? EventTypeId { get; set; }
        public virtual EventType EventType { get; set; }
        public int? VacancyId { get; set; }
        public virtual Vacancy Vacancy { get; set; }
        public int? CandidateId { get; set; }
        public virtual Candidate Candidate { get; set; }
    }
}