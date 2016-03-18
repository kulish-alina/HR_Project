using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BotDomain.Entities;
using BotDomain.Repositories;
using BotData.DumbData;

namespace Data.DumbData
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public User GetByEmail()
        {
            throw new NotImplementedException();
        }
    }
}
