using Domain.Entities;
using Domain.Entities.Enum;
using Domain.Entities.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Domain.DTO.DTOModels
{
    public class LanguageSkillDTO
    {
        public LanguageSkillDTO(LanguageSkill langSkill)
        {
            Id = langSkill.Id;
            EditTime = langSkill.EditTime;
            State = langSkill.State;
            LanguageId = langSkill.Language == null ? 0 : langSkill.Language.Id;
            LanguageLevel = langSkill.LanguageLevel;
        }

        public LanguageSkillDTO()
        {

        }

        public int Id { get; set; }
        public DateTime EditTime { get; set; }
        public EntityState State { get; set; }
        public int LanguageId { get; set; }
        public LanguageLevel LanguageLevel { get; set; }
    }
}