using Domain.Entities;

namespace DAL.Mapping
{
    public class RelocationPlaceConfiguration : BaseEntityConfiguration<RelocationPlace>
    {
        public RelocationPlaceConfiguration()
        {
            HasRequired(x => x.Country).WithMany().HasForeignKey(x => x.CountryId);
            HasOptional(x => x.City).WithMany().HasForeignKey(x => x.CityId);
        }
    }
}
