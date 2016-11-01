using Domain.Entities;
using System;
using System.Threading.Tasks;

namespace DAL.Infrastructure
{
    public interface IUserRepository : IRepository<User>
    {
        User Get(string login, string password);
        User Get(Func<User, bool> predicate);
        Task<User> GetAsync(string login, string password);
    }
}