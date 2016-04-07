using Domain.Entities.Enum;

namespace Domain.Entities
{
    public class CandidateSource : BaseEntity
    {
        public Source Source { get; set; }
        public string Path { get; set; }
    }
}
