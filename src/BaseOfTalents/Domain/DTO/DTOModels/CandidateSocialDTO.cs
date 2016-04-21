using Domain.Entities;
using Domain.Entities.Enum;
using System;

namespace Domain.DTO.DTOModels
{
    public class CandidateSocialDTO
    {
        public CandidateSocialDTO(CandidateSocial candSocial)
        {
            Id = candSocial.Id;
            EditTime = candSocial.EditTime;
            State = candSocial.State;
            SocialNetworkId = candSocial.SocialNetwork == null ? 0 : candSocial.SocialNetwork.Id;
            Path = candSocial.Path;
        }

        public CandidateSocialDTO()
        {

        }

        public int Id { get; set; }
        public DateTime EditTime { get; set; }
        public EntityState State { get; set; }
        public int SocialNetworkId { get; set; }
        public string Path { get; set; }
    }
}