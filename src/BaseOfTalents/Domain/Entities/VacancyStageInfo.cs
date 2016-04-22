using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class VacancyStageInfo : BaseEntity
    {
        public virtual VacancyStage VacancyStage { get; set; }

        public virtual Comment Comment { get; set; }

        public int CandidateId { get; set; }
        public virtual Candidate Candidate { get; set; }
    }
}