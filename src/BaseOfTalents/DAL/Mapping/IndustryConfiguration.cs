using BaseOfTalents.Domain.Entities.Enum.Setup;

namespace BaseOfTalents.DAL.Mapping
{
    public class IndustryConfiguration : BaseEntityConfiguration<Industry>
    {
        public IndustryConfiguration()
        {
            Property(i => i.Title).IsRequired();
        }
    }
}