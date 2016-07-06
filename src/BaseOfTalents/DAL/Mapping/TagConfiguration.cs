using Domain.Entities.Enum.Setup;

namespace DAL.Mapping
{
    public class TagConfiguration : BaseEntityConfiguration<Tag>
    {
        public TagConfiguration()
        {
            Property(t => t.Title).IsRequired();
        }
    }
}