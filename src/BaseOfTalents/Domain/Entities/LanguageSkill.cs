using Domain.Entities.Enum;
using Domain.Entities.Enum.Setup;

namespace Domain.Entities
{
    public class LanguageSkill : BaseEntity
    {
        public LanguageLevel? LanguageLevel { get; set; }

        public int LanguageId { get; set; }
        public virtual Language Language { get; set; }
    }
}