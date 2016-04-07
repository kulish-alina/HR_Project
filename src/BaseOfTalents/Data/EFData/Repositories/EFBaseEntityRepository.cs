using Domain.Entities;
using Domain.Entities.Enum;
using Domain.Entities.Setup;
using System.Linq.Expressions;

namespace Data.EFData.Repositories
{
    public class EFBaseEntityRepository
    {
        protected BOTContext _context = new BOTContext();
    }
}
