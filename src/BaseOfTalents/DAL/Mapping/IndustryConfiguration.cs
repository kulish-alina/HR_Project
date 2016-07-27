using Domain.Entities.Enum.Setup;

namespace DAL.Mapping
{
    public class IndustryConfiguration : BaseEntityConfiguration<Industry>
    {
        public IndustryConfiguration()
        {
            Property(i => i.Title).IsRequired();
        }
    }
}