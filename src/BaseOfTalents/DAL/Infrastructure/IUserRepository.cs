using System;
using System.Threading.Tasks;
using Domain.Entities;

namespace DAL.Infrastructure
{
    public interface IUserRepository : IRepository<User>
    {
        User Get(Func<User, bool> predicate);
        User Get(string login);
        Task<User> GetAsync(string login);
    }
}