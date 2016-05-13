using Domain.DTO.DTOModels;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EFData.Extentions
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
