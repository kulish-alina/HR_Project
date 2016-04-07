using Domain.Entities;
using Domain.Entities.Enum;
using Domain.Entities.Setup;
using Domain.Repositories;
using System.Linq.Expressions;

namespace Data.DumbData.Repositories
{
    class DummyUserRepository : DummyBaseEntityRepository<User>, IUserRepository
    {
        public DummyUserRepository(DummyBotContext context) : base(context)
        {

        }
       
    }
}
