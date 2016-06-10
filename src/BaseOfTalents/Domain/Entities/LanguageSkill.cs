using BaseOfTalents.Domain.Entities.Enum;
using BaseOfTalents.Domain.Entities.Enum.Setup;

namespace BaseOfTalents.Domain.Entities
{
    public class LanguageSkill : BaseEntity
    {
        public LanguageLevel LanguageLevel { get; set; }

        public int LanguageId { get; set; }
        public virtual Language Language { get; set; }
    }
}