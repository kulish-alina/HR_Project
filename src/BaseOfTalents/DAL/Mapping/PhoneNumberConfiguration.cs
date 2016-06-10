using BaseOfTalents.Domain.Entities;

namespace BaseOfTalents.DAL.Mapping
{
    public class PhoneNumberConfiguration : BaseEntityConfiguration<PhoneNumber>
    {
        public PhoneNumberConfiguration()
        {
            Property(p => p.Number).IsRequired();
        }
    }
}