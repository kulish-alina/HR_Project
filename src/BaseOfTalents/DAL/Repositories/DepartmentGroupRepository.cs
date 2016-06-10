using BaseOfTalents.DAL.Infrastructure;
using BaseOfTalents.DAL.Repositories;
using BaseOfTalents.Domain.Entities.Enum.Setup;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class DepartmentGroupRepository : BaseRepository<DepartmentGroup>, IDepartmentGroupRepository
    {
        public DepartmentGroupRepository(DbContext context) : base(context)
        {
        }
    }
}
