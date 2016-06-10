using BaseOfTalents.Domain.Entities;
using Domain.DTO.DTOModels;

namespace DAL.Extensions
{
    public static class LanguageSkillExtension
    {
        public static void Update(this LanguageSkill domain, LanguageSkillDTO dto)
        {
            domain.LanguageLevel = dto.LanguageLevel;
            domain.LanguageId = dto.LanguageId;
            domain.State = dto.State;
        }
    }
}
