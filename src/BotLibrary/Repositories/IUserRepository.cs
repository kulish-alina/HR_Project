using BotDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotDomain.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        User GetByEmail();
    }
}
