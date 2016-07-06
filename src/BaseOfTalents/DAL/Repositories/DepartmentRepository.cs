using DAL.Infrastructure;
using Domain.Entities.Enum.Setup;
using System.Data.Entity;

namespace DAL.Repositories
{
    public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(DbContext context) : base(context)
        {
        }
    }
}