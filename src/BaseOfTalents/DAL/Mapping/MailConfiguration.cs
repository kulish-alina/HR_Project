using Domain.Entities;

namespace DAL.Mapping
{
    class MailConfiguration : BaseEntityConfiguration<MailContent>
    {
        public MailConfiguration()
        {
            Property(mail => mail.Invitation).IsRequired();
            Property(mail => mail.Body).IsRequired();
            Property(mail => mail.Farewell).IsRequired();
            Property(mail => mail.Subject).IsRequired();
        }
    }
}
