using Domain.Entities.Enum;
using System;

namespace WebApi.DTO.DTOModels
{
    public class SocialNetworkDTO
    {
        public int Id { get; set; }
        public DateTime EditTime { get; set; }
        public EntityState State { get; set; }
        public string Title { get; set; }
        public string ImagePath { get; set; }
    }
}