using BaseOfTalents.DAL.Repositories;
using BaseOfTalents.Domain.Entities.Enum.Setup;
using DAL.Infrastructure;
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
