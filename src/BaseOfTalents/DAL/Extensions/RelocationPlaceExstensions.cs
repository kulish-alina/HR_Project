using DAL.DTO;
using Domain.Entities;

namespace DAL.Extensions
{
    public static class RelocationPlaceExstensions
    {
        public static void Update(this RelocationPlace destination, RelocationPlaceDTO source)
        {
            destination.CountryId = source.CountryId;
            destination.CityId = source.CityId;
        }
    }
}
