using BaseOfTalents.Domain.Entities.Enum;

namespace BaseOfTalents.Domain.Entities
{
    public class CandidateSource : BaseEntity
    {
        public Source Source { get; set; }
        public string Path { get; set; }

        public virtual Candidate Candidate { get; set; }
    }
}