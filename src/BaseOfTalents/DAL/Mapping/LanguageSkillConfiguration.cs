using BaseOfTalents.Domain.Entities;

namespace BaseOfTalents.DAL.Mapping
{
    public class LanguageSkillConfiguration : BaseEntityConfiguration<LanguageSkill>
    {
        public LanguageSkillConfiguration()
        {
            Property(ls => ls.LanguageLevel).IsRequired();

            HasRequired(ls => ls.Language).WithMany().HasForeignKey(ls => ls.LanguageId);
        }
    }
}