using Domain.Entities.Enum;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class CandidateSource : BaseEntity
    {
        public Source Source { get; set; }
        public string Path { get; set; }

    }
}
