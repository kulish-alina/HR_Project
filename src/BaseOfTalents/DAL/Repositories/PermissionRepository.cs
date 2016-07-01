using DAL.Infrastructure;
using Domain.Entities.Enum.Setup;
using System.Data.Entity;

namespace DAL.Repositories
{
    public class PermissionRepository : BaseRepository<Permission>, IPermissionRepository
    {
        public PermissionRepository(DbContext context) : base(context)
        {

        }
    }
}
