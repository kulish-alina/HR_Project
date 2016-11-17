using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DAL.DTO;
using DAL.Infrastructure;
using Domain.Entities;

namespace DAL.Services
{
    public abstract class BaseService<DomainEntity, DTO>
        where DomainEntity : BaseEntity, new()
        where DTO : BaseEntityDTO, new()
    {
        protected readonly IUnitOfWork uow;
        protected readonly IRepository<DomainEntity> currentRepo;
        private ICountryRepository countryRepo;

        public BaseService(IUnitOfWork uow, IRepository<DomainEntity> currentRepo)
        {
            this.uow = uow;
            this.currentRepo = currentRepo;
        }

        public virtual DTO Add(DTO entity)
        {
            var entityToAdd = DTOService.ToEntity<DTO, DomainEntity>(entity);
            currentRepo.Insert(entityToAdd);
            uow.Commit();
            return DTOService.ToDTO<DomainEntity, DTO>(entityToAdd);
        }

        public virtual bool Delete(int id)
        {
            bool deleteResult;
            var entityToDelete = currentRepo.GetByID(id);
            if (entityToDelete == null)
            {
                deleteResult = false;
            }
            else
            {
                currentRepo.Delete(id);
                uow.Commit();
                deleteResult = true;
            }
            return deleteResult;
        }

        public virtual DTO Get(int id)
        {
            var entity = currentRepo.GetByID(id);
            return DTOService.ToDTO<DomainEntity, DTO>(entity);
        }

        public IEnumerable<DomainEntity> Get(IEnumerable<int> ids)
        {
            return currentRepo.Get(new List<Expression<Func<DomainEntity, bool>>>
            {
                x => ids.Contains(x.Id)
            });
        }

        public virtual IEnumerable<DTO> Get()
        {
            var entities = currentRepo.Get();
            return entities.Select(en => DTOService.ToDTO<DomainEntity, DTO>(en));
        }

        public virtual DTO Update(DTO entity)
        {
            var changedDomainEntity = DTOService.ToEntity<DTO, DomainEntity>(entity);
            currentRepo.Update(changedDomainEntity);
            uow.Commit();
            return DTOService.ToDTO<DomainEntity, DTO>(changedDomainEntity);

        }
    }
}
