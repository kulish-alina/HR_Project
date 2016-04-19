using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Domain.Entities.Enum;
using Domain.Entities.Setup;
using System.Linq.Expressions;
using Domain.Repositories;

namespace Data.DumbData.Repositories
{
    public class DummyBaseEntityRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity, new()
    {
        protected static IList<TEntity> Collection;
        protected DummyBotContext _context;

        public DummyBaseEntityRepository(DummyBotContext context)
        {
            _context = context;
        }

        public TEntity Get(int id)
        {
            return Collection.FirstOrDefault(x => x.Id == id);
        }

        public void Add(TEntity entity)
        {
            if (!Collection.Any(x => x.Id == entity.Id))
            {
                if (entity.Id == 0)
                {
                    entity.Id = Collection.Count + 1;
                }
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
            Collection.First(x => x == entity).State = EntityState.Inactive;
        }

        public void Update(TEntity entity)
        {
            Collection[Collection.IndexOf(Collection.First(x => x.Id == entity.Id))] = entity;
        }

    }
}
