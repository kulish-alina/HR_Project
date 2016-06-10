using System;
using Domain.Entities.Enum;
using BaseOfTalents.Domain.Entities.Enum;

namespace Domain.DTO.DTOModels
{
    public class LanguageSkillDTO : BaseEntityDTO
    {
        public int LanguageId { get; set; }
        public LanguageLevel LanguageLevel { get; set; }
    }
}