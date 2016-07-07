using Domain.Entities;

namespace DAL.Mapping
{
    public class EventConfiguration : BaseEntityConfiguration<Event>
    {
        public EventConfiguration()
        {
            Property(e => e.EventDate).IsRequired();
            Property(e => e.Description).IsRequired();
            HasRequired(e => e.Responsible).WithMany().HasForeignKey(e => e.ResponsibleId);

            HasOptional(e => e.Vacancy).WithMany().HasForeignKey(x => x.VacancyId);
            HasOptional(e => e.Candidate).WithMany().HasForeignKey(x => x.CandidateId);
            HasOptional(e => e.EventType).WithMany().HasForeignKey(e => e.EventTypeId);
        }
    }
}