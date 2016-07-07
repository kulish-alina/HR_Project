namespace Domain.Entities
{
    public class VacancyStageInfo : BaseEntity
    {
        public virtual VacancyStage VacancyStage { get; set; }

        public int CandidateId { get; set; }
        public virtual Candidate Candidate { get; set; }

        public int VacancyId { get; set; }
        public virtual Vacancy Vacancy { get; set; }

        public virtual Comment Comment { get; set; }
    }
}