using BaseOfTalents.Domain.Entities.Enum;
using Domain.Entities.Enum;

namespace Domain.DTO.DTOModels
{
    public class CandidateSourceDTO : BaseEntityDTO
    {
        public Source Source { get; set; }
        public string Path { get; set; }
    }
}
