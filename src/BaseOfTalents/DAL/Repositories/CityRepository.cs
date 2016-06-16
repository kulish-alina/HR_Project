using BaseOfTalents.DAL.Infrastructure;
using BaseOfTalents.Domain.Entities.Enum.Setup;
using System.Data.Entity;

namespace BaseOfTalents.DAL.Repositories
{
    public class CityRepository : BaseRepository<Location>, ILocationRepository
    {
        public CityRepository(DbContext context) : base(context)
        {
        }
    }
}