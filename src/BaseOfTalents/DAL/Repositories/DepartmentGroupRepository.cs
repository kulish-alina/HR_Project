using DAL.Infrastructure;
using Domain.Entities.Enum.Setup;
using System.Data.Entity;

namespace DAL.Repositories
{
    public class DepartmentGroupRepository : BaseRepository<DepartmentGroup>, IDepartmentGroupRepository
    {
        public DepartmentGroupRepository(DbContext context) : base(context)
        {
        }
    }
}
