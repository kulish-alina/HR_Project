using BaseOfTalents.Domain.Entities;

namespace BaseOfTalents.DAL.Mapping
{
    public class EventConfiguration : BaseEntityConfiguration<Event>
    {
        public EventConfiguration()
        {
            Property(e => e.EventDate).IsRequired();
            Property(e => e.Description).IsRequired();

            HasOptional(e => e.Vacancy).WithMany();
            HasOptional(e => e.Candidate).WithMany();

            HasRequired(e => e.EventType).WithMany().HasForeignKey(e => e.EventTypeId);
            HasRequired(e => e.Responsible).WithMany().HasForeignKey(e => e.ResponsibleId);
        }
    }
}