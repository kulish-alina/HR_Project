using BaseOfTalents.Domain.Entities.Enum.Setup;

namespace BaseOfTalents.DAL.Mapping
{
    public class EventTypeConfiguration : BaseEntityConfiguration<EventType>
    {
        public EventTypeConfiguration()
        {
            Property(et => et.ImagePath).IsOptional();
            Property(et => et.Title).IsRequired();
        }
    }
}