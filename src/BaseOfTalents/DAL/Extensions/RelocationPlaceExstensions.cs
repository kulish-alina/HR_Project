using BaseOfTalents.DAL.Infrastructure;
using BaseOfTalents.Domain.Entities.Enum.Setup;
using DAL.DTO;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Extensions
{
    public static class RelocationPlaceExstensions 
    {
        public static void Update(this RelocationPlace destination, RelocationPlaceDTO source, IRepository<City> locationRepo)
        {
            destination.CountryId = source.CountryId;
            PerformCitiesSaving(destination, source, locationRepo);
            destination.Cities.ToList().ForEach(x =>
            {
                if(x.CountryId!=destination.CountryId)
                {
                    throw new Exception("City not from that country");
                }
            });
        }

        private static void PerformCitiesSaving(RelocationPlace destination, RelocationPlaceDTO source, IRepository<City> locationRepo)
        {
            destination.Cities.Clear();
            destination.Cities = source.LocationIds.Select(x => locationRepo.GetByID(x)).ToList();
        }
    }
}
