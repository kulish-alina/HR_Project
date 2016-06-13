using BaseOfTalents.DAL.Infrastructure;
using BaseOfTalents.Domain.Entities;
using DAL.Extensions;
using Domain.DTO.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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

        public IEnumerable<EventDTO> Get(IEnumerable<int> userIds, int month, int year)
        {
            var domainEvents = new List<Event>();
            userIds.ToList().ForEach(x =>
            {
                var filters = new List<Expression<Func<Event, bool>>>();
                filters.Add(e => e.ResponsibleId == x);
                filters.Add(e => e.EventDate.Month == month);
                filters.Add(e => e.EventDate.Year == year);
                var foundedEvents = uow.EventRepo.Get(filters);
                domainEvents.AddRange(foundedEvents);
            });
            var eventsDto = domainEvents.Select(x => DTOService.ToDTO<Event, EventDTO>(x));
            return eventsDto;
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
