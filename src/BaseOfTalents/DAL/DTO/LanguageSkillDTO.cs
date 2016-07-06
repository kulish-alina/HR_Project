using Domain.Entities.Enum;

namespace DAL.DTO
{
    public class LanguageSkillDTO : BaseEntityDTO
    {
        public int LanguageId { get; set; }
        public LanguageLevel? LanguageLevel { get; set; }
    }
}