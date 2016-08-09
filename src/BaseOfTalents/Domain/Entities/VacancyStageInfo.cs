using Domain.Entities.Enum;
using Domain.Entities.Enum.Setup;

namespace Domain.Entities
{
    public class VacancyStageInfo : BaseEntity
    {
        public int StageId { get; set; }
        public virtual Stage Stage { get; set; }

        public StageState StageState { get; set; }

        public int CandidateId { get; set; }
        public virtual Candidate Candidate { get; set; }

        public int VacancyId { get; set; }
        public virtual Vacancy Vacancy { get; set; }

        public virtual Comment Comment { get; set; }
    }
}