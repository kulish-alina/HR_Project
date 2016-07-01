using DAL.Infrastructure;
using Domain.Entities.Enum.Setup;
using System.Data.Entity;

namespace DAL.Repositories
{
    public class SourceRepository : BaseRepository<Source>, ISourceRepository
    {
        public SourceRepository(DbContext context) : base(context)
        {
        }
    }
}
