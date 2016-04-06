using BotData.Abstract;
using BotLibrary.Entities;
using BotLibrary.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotData.DumbData.Repositories
{
    class UserRepository : BaseEntityRepository<User>, IUserRepository
    {
        public UserRepository(IContext context) : base(context)
        {

        }
       
    }
}
