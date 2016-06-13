using BaseOfTalents.DAL.Infrastructure;
using BaseOfTalents.Domain.Entities;
using DAL.Extensions;
using Domain.DTO.DTOModels;
using System;

namespace DAL.Services
{
    public class EventService
    {
        IUnitOfWork uow;
        public EventService(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public EventDTO Get(int id)
        {
            var entity = uow.EventRepo.GetByID(id);
            return DTOService.ToDTO<Event, EventDTO>(entity);
        }

        public EventDTO Add(EventDTO eventToAdd)
        {
            Event newEvent = new Event();
            newEvent.Update(eventToAdd);
            uow.EventRepo.Insert(newEvent);
            uow.Commit();
            return DTOService.ToDTO<Event, EventDTO>(newEvent);
        }

        public object Get(object searchParameters)
        {
            throw new NotImplementedException();
            //return base.Get(searchParameters);
        }

        public EventDTO Update(EventDTO eventToChange)
        {
            Event domainEvent = uow.EventRepo.GetByID(eventToChange.Id);
            domainEvent.Update(eventToChange);
            uow.EventRepo.Update(domainEvent);
            uow.Commit();
            return DTOService.ToDTO<Event, EventDTO>(domainEvent);
        }
        public bool Delete(int id)
        {
            bool deleteResult;
            var entityToDelete = uow.EventRepo.GetByID(id);
            if (entityToDelete == null)
            {
                deleteResult = false;
            }
            else {
                uow.EventRepo.Delete(id);
                uow.Commit();
                deleteResult = true;
            }
            return deleteResult;
        }
    }
}
