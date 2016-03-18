using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotDomain.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        List<TEntity> GetAll();

        TEntity FindById(int id);

        void Add(TEntity entity);
        void Update(TEntity entity);
        void Remove(TEntity entity);
    }
}
