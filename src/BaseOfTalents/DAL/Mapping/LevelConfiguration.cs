using BaseOfTalents.Domain.Entities.Enum.Setup;

namespace BaseOfTalents.DAL.Mapping
{
    public class LevelConfiguration : BaseEntityConfiguration<Level>
    {
        public LevelConfiguration()
        {
            Property(l => l.Title).IsRequired();
        }
    }
}