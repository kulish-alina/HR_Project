using BaseOfTalents.Domain.Entities.Enum.Setup;

namespace BaseOfTalents.DAL.Mapping
{
    public class CityConfiguration : BaseEntityConfiguration<City>
    {
        public CityConfiguration()
        {
            Property(l => l.Title).IsRequired();
            HasRequired(l => l.Country).WithMany().HasForeignKey(l => l.CountryId);
        }
    }
}