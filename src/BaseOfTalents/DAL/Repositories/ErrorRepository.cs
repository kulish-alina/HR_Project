using BaseOfTalents.DAL.Infrastructure;
using BaseOfTalents.Domain.Entities;
using System.Data.Entity;

namespace BaseOfTalents.DAL.Repositories
{
    public class ErrorRepository : BaseRepository<Error>, IErrorRepository
    {
        public ErrorRepository(DbContext context) : base(context)
        {
        }
    }
}