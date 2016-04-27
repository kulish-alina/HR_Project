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
        private BOTContext dataContext;
        #region Properties
        protected IDbFactory DbFactory
        {
            get;
            private set;
        }
        protected BOTContext DbContext
        {
            get { return dataContext ?? (dataContext = DbFactory.Init()); }
        }

        public EFBaseEntityRepository(IDbFactory dbFactory)
        {
            DbFactory = dbFactory;

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
            return GetAll().FirstOrDefault(x => x.Id == id);
        }
        public virtual IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            return DbContext.Set<TEntity>().Where(predicate);
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
        public virtual void Remove(TEntity entity)
        {
            entity.State = Domain.Entities.Enum.EntityState.Inactive;
            DbContext.Entry(entity).State = System.Data.Entity.EntityState.Modified;
        }
    }
}
