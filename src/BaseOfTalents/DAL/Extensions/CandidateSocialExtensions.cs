using BaseOfTalents.Domain.Entities;
using Domain.DTO.DTOModels;

namespace DAL.Extensions
{ 
    public static class CandidateSocialExtensions
    {
        public static void Update(this CandidateSocial domain, CandidateSocialDTO dto)
        {
            domain.Path = dto.Path;
            domain.SocialNetworkId = dto.SocialNetworkId;
            domain.State = dto.State;
        }
    }
}
