using DAL.Infrastructure;
using Domain.Entities.Enum.Setup;
using System.Data.Entity;

namespace DAL.Repositories
{
    public class IndustryRepository : BaseRepository<Industry>, IIndustryRepository
    {
        public IndustryRepository(DbContext context) : base(context)
        {

        }
    }
}
