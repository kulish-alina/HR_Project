using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotDomain.Repositories;

namespace BotData.DumbData
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        public void Add(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public TEntity FindById(int id)
        {
            throw new NotImplementedException();
        }

        public List<TEntity> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Remove(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Update(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
