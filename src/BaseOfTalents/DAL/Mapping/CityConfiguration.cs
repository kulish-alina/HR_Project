using Domain.Entities.Enum.Setup;

namespace DAL.Mapping
{
    public class CityConfiguration : BaseEntityConfiguration<City>
    {
        public CityConfiguration()
        {
            Property(l => l.Title).IsRequired();
            HasRequired(l => l.Country).WithMany().HasForeignKey(l => l.CountryId);
            HasMany(x => x.RelocationPlaces).WithOptional().HasForeignKey(x => x.CityId);
        }
    }
}