using BaseOfTalents.Domain.Entities.Enum.Setup;

namespace BaseOfTalents.DAL.Mapping
{
    public class TagConfiguration : BaseEntityConfiguration<Tag>
    {
        public TagConfiguration()
        {
            Property(t => t.Title).IsRequired();
        }
    }
}