using BotLibrary.Entities;
using BotLibrary.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using BotLibrary.Entities.Enum;
using BotData.Abstract;

namespace BotData.DumbData.Repositories
{
    public class BaseEntityRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity, new()
    {
        protected static IList<TEntity> Collection;
        protected IContext _context;

        public BaseEntityRepository(IContext context)
        {
            _context = context;
        }

        public TEntity Get(int id)
        {
            return Collection.FirstOrDefault(x => x.Id == id);
        }

        public void Add(TEntity entity)
        {
            if (Collection.Any(x => x == entity))
            {
                entity.Id = Collection.OrderBy(x => x.Id).Last().Id + 1;
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
