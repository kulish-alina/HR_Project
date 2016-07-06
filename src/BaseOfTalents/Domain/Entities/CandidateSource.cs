using Domain.Entities.Enum.Setup;

namespace Domain.Entities
{
    public class CandidateSource : BaseEntity
    {
        public string Path { get; set; }

        public int SourceId { get; set; }
        public virtual Source Source { get; set; }

        public virtual Candidate Candidate { get; set; }
    }
}