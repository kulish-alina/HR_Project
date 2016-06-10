using BaseOfTalents.Domain.Entities.Enum.Setup;

namespace BaseOfTalents.DAL.Mapping
{
    public class SkillConfiguration : BaseEntityConfiguration<Skill>
    {
        public SkillConfiguration()
        {
            Property(s => s.Title);
        }
    }
}