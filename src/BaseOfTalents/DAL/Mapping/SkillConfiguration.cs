using Domain.Entities.Enum.Setup;

namespace DAL.Mapping
{
    public class SkillConfiguration : BaseEntityConfiguration<Skill>
    {
        public SkillConfiguration()
        {
            Property(s => s.Title);
        }
    }
}