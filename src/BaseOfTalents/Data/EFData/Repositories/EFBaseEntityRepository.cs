using Domain.Entities;
using Domain.Entities.Enum;
using Domain.Entities.Setup;
using Domain.Repositories;
using System.Linq.Expressions;
using System;
using System.Linq;
using Data.Infrastructure;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Data.EFData.Repositories
{

    public class EFBaseEntityRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity, new()
    {
        #region Properties
        IUnitOfWork uov = null;

        protected DbContext DbContext
        {
            get { return uov.Context; }
        }

        public EFBaseEntityRepository(IUnitOfWork unitOfWork)
        {
            this.uov = unitOfWork;
        }
        #endregion

        public virtual IQueryable<TEntity> GetAll()
        {
            return DbContext.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> All
        {
            get
            {
                return GetAll();
            }
        }

        public virtual IQueryable<TEntity> AllIncluding(params Expression<Func<TEntity, object>>[]
       includeProperties)
        {
            IQueryable<TEntity> query = DbContext.Set<TEntity>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public virtual TEntity Get(int id)
        {
            return GetAll().SingleOrDefault(x => x.Id == id);
        }
        public virtual IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate);
        }
        public virtual void Add(TEntity entity)
        {
            DbEntityEntry dbEntityEntry = DbContext.Entry<TEntity>(entity);
            DbContext.Set<TEntity>().Add(entity);
        }
        public virtual void Update(TEntity entity)
        {
            DbEntityEntry dbEntityEntry = DbContext.Entry<TEntity>(entity);
            dbEntityEntry.State = System.Data.Entity.EntityState.Modified;
        }

        public virtual void Remove(int entityId)
        {
            var entityToRemove = Get(entityId);
            Remove(entityToRemove);
        }

        public virtual void Remove(TEntity entity)
        {
            var attachedEntity = DbContext.Set<TEntity>().Attach(entity);
            DbContext.Set<TEntity>().Remove(attachedEntity);
        }

        public void Commit()
        {
            uov.Commit();
        }
    }
}
