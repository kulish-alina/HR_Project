using Domain.Entities;

namespace DAL.Mapping
{
    public class PhoneNumberConfiguration : BaseEntityConfiguration<PhoneNumber>
    {
        public PhoneNumberConfiguration()
        {
            Property(p => p.Number).IsRequired();
        }
    }
}