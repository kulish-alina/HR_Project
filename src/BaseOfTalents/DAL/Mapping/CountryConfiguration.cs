using Domain.Entities.Enum.Setup;

namespace DAL.Mapping
{
    public class CountryConfiguration : BaseEntityConfiguration<Country>
    {
        public CountryConfiguration()
        {
            Property(c => c.Title).IsRequired();

            HasMany(x => x.Cities).WithRequired(x => x.Country).HasForeignKey(x => x.CountryId);
        }
    }
}