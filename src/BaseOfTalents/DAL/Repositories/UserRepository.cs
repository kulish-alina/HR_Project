using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DAL.Infrastructure;
using Domain.Entities;

namespace DAL.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(DbContext context) : base(context)
        {
        }

        public User Get(Func<User, bool> predicate)
        {
            return dbSet.FirstOrDefault(predicate);
        }

        /// <summary>
        /// The function to get user of login
        /// </summary>
        /// <param name="login">String representing an application user login</param>
        /// <returns>A user that matches applied login, if there is no such user returns null</returns>
        public User Get(string login)
        {
            return dbSet.Include(user => user.Password).FirstOrDefault(user =>
                 user.Login == login);
        }
        /// <summary>
        /// The function to get user of login async
        /// </summary>
        /// <param name="login">String representing an application user login</param>
        /// <returns>A user that matches applied login, if there is no such user returns null</returns>
        public async Task<User> GetAsync(string login)
        {
            return await dbSet.Include(user => user.Password).FirstOrDefaultAsync(user =>
                 user.Login == login);
        }
    }
}