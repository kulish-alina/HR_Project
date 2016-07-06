using Domain.Entities;
using System.Threading.Tasks;

namespace DAL.Infrastructure
{
    public interface IUserRepository : IRepository<User>
    {
        User Get(string login, string password);
        Task<User> GetAsync(string login, string password);
    }
}