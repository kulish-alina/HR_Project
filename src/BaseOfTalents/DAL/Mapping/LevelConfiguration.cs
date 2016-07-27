using Domain.Entities.Enum.Setup;

namespace DAL.Mapping
{
    public class LevelConfiguration : BaseEntityConfiguration<Level>
    {
        public LevelConfiguration()
        {
            Property(l => l.Title).IsRequired();
        }
    }
}