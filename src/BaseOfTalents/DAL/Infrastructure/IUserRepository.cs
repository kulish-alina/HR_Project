using System.Threading.Tasks;
using BaseOfTalents.Domain.Entities;

namespace BaseOfTalents.DAL.Infrastructure
{
    public interface IUserRepository : IRepository<User>
    {
        User Get(string login, string password);
        Task<User> GetAsync(string login, string password);
    }
}