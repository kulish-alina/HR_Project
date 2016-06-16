using BaseOfTalents.DAL.Infrastructure;
using BaseOfTalents.Domain.Entities;
using System.Data.Entity;

namespace BaseOfTalents.DAL.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(DbContext context) : base(context)
        {
        }
    }
}