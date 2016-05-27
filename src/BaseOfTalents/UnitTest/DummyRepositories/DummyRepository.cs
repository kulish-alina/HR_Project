using Domain.Entities;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace UnitTest.DummyRepositories
{
    public class DummyRepository<TEntity> : IRepository<TEntity>
        where TEntity : BaseEntity, new()
    {
        protected static List<TEntity> Collection;

        public void Add(TEntity entity)
        {
            entity.Id = Collection.Count + 1;
            entity.CreatedOn = DateTime.Now;
            entity.LastModified = DateTime.Now;
            Collection.Add(entity);
        }

        public IQueryable<TEntity> AllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public void Commit()
        {
        }

        public IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            return Collection.AsQueryable().Where(predicate);
        }

        public TEntity Get(int id)
        {
            return Collection.FirstOrDefault(x => x.Id == id);
        }

        public IQueryable<TEntity> GetAll()
        {
            return Collection.AsQueryable();
        }

        public void Remove(TEntity entity)
        {
            Remove(entity.Id);
        }

        public void Remove(int entityId)
        {
            Collection.Remove(Collection.First(x => x.Id == entityId));
        }

        public void Update(TEntity entity)
        {
            Collection[Collection.IndexOf(Collection.First(x => x.Id == entity.Id))] = entity;
        }
    }
}
