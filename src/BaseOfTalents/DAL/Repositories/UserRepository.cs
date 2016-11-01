using DAL.Infrastructure;
using Domain.Entities;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

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
        /// The function to get user of login and password
        /// </summary>
        /// <param name="login">String representing an application user login</param>
        /// <param name="password">String that contains the password hash to search with</param>
        /// <returns>A user that matches applied login and password, if there is no such user returns null</returns>
        public User Get(string login, string password)
        {
            return dbSet.FirstOrDefault(user =>
                 user.Login == login &&
                 user.Password == password);
        }
        /// <summary>
        /// The function to get user of login and password async
        /// </summary>
        /// <param name="login">String representing an application user login</param>
        /// <param name="password">String that contains the password hash to search with</param>
        /// <returns>A user that matches applied login and password, if there is no such user returns null</returns>
        public async Task<User> GetAsync(string login, string password)
        {
            return await dbSet.FirstOrDefaultAsync(user =>
                 user.Login == login &&
                 user.Password == password);
        }
    }
}