using Domain.Entities;
using Domain.Entities.Enum;
using Domain.Entities.Setup;
using Domain.Repositories;
using System.Linq.Expressions;
using System;
using System.Linq;

namespace Data.EFData.Repositories
{

    public class EFBaseEntityRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity, new()
    {

        protected BOTContext _context = new BOTContext();

        public void Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            _context.SaveChanges();
        }

        public IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().Where(predicate);
        }

        public TEntity Get(int id)
        {
            return _context.Set<TEntity>().FirstOrDefault(x=>x.Id==id);
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return _context.Set<TEntity>().AsQueryable();
        }

        public void Remove(TEntity entity)
        {
            entity.State = EntityState.Inactive;
            _context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            var attachedEntity = _context.Set<TEntity>().Attach(entity);
            _context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
