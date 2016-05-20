using Domain.DTO.DTOModels;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EFData.Extentions
{
    public static class LanguageSkillExtensions
    {
        public static void Update(this LanguageSkill domain, LanguageSkillDTO dto)
        {
            domain.LanguageLevel = dto.LanguageLevel;
            domain.LanguageId = dto.LanguageId;
            domain.State = dto.State;
        }
    }
}
