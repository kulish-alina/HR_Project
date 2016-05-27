using Data.Infrastructure;
using Domain.DTO.DTOModels;
using Domain.Entities;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public abstract class ControllerService<DomainEntity,ViewModel> : IControllerService<DomainEntity, ViewModel> 
        where DomainEntity : BaseEntity, new()
        where ViewModel : BaseEntityDTO, new ()
    {
        private HttpRequestMessage currentRequest;

        protected virtual IRepository<DomainEntity> entityRepository { get; set; }

        public ControllerService(IRepository<DomainEntity> repository)
        {
            entityRepository = repository;
        }

        public virtual ViewModel GetById(int id)
        {
            var foundedEntity = entityRepository.Get(id);
            return DTOService.ToDTO<DomainEntity, ViewModel>(foundedEntity);
        }

        public virtual ViewModel Add(ViewModel entity)
        {
            var newEntity = DTOService.ToEntity<ViewModel, DomainEntity>(entity);
            entityRepository.Add(newEntity);
            entityRepository.Commit();
            return DTOService.ToDTO<DomainEntity, ViewModel> (newEntity);
        }

        public virtual void Remove(int id)
        {
            var entityToRemove = entityRepository.Get(id);
            if (entityToRemove != null)
            {
                entityRepository.Remove(entityToRemove);
                entityRepository.Commit();
            }
            else
            {
                throw new MissingMemberException();
            }
        }

        public virtual ViewModel Put(ViewModel entity)
        {
            var changedDomainEntity = DTOService.ToEntity<ViewModel, DomainEntity>(entity);
            entityRepository.Update(changedDomainEntity);
            entityRepository.Commit();
            return DTOService.ToDTO<DomainEntity, ViewModel>(changedDomainEntity);
        }

        public virtual IEnumerable<ViewModel> GetAll()
        {
            var entitiesQuery = entityRepository.GetAll().OrderBy(x => x.Id);
            return entitiesQuery.ToList().Select(x => DTOService.ToDTO<DomainEntity, ViewModel>(x));
        }

        public abstract object Search(object searchParams);
    }
}
