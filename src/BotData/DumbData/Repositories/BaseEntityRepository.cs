using BotLibrary.Entities;
using BotLibrary.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using BotLibrary.Entities.Enum;

namespace BotData.DumbData.Repositories
{
    public class BaseEntityRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity, new()
    {
        protected static IList<TEntity> Collection;

        public void Add(TEntity entity)
        {
            if (Collection.Any(x => x == entity))
            {
                Collection.Add(entity);
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            return Collection.AsQueryable().Where(predicate);
        }

        public IQueryable<TEntity> GetAll()
        {
            return Collection.AsQueryable();
        }

        public void Remove(TEntity entity)
        {
            Collection.First(x => x == entity).State = EntityState.Active;
        }

        public void Update(TEntity entity)
        {
            Collection[Collection.IndexOf(Collection.First(x => x.Id == entity.Id))] = entity;
        }

    }
}
