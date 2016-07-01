using DAL.DTO;
using DAL.Extensions;
using DAL.Infrastructure;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DAL.Services
{
    public partial class EventService
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

        public IEnumerable<EventDTO> GetByCandidateId(int candidateId)
        {
            var domainEvents = new List<Event>();
            var filters = new List<Expression<Func<Event, bool>>>();
            filters.Add(x => x.CandidateId == candidateId);
            return uow.EventRepo.Get(filters).Select(x => DTOService.ToDTO<Event, EventDTO>(x));
        }

        public IEnumerable<EventDTO> Get(IEnumerable<int> userIds, DateTime startDate, DateTime? endDate)
        {
            var domainEvents = new List<Event>();

            var clearedStartDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, 0, 0, 0);
            var clearedEndDate = endDate.HasValue ? new DateTime(endDate.Value.Year, endDate.Value.Month, endDate.Value.Day, 23, 59, 59) : endDate;

            if (IsNeededEventsForAMonth(clearedStartDate, clearedEndDate))
            {
                domainEvents = EventsForAMonthForAUsers(userIds, clearedStartDate);
            }
            else
            {
                domainEvents = EventsForAUsersForADateBetween(userIds, clearedStartDate, clearedEndDate);
            }
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

    public partial class EventService
    {
        private List<Event> EventsForAUsersForADateBetween(IEnumerable<int> userIds, DateTime startDate, DateTime? endDate)
        {
            var foundedEvents = new List<Event>();
            userIds.ToList().ForEach(x =>
            {
                foundedEvents.AddRange(EventsForADateBetween(x, startDate, endDate));
            });
            return foundedEvents;
        }
        private List<Event> EventsForAMonthForAUsers(IEnumerable<int> userIds, DateTime startDate)
        {
            var foundedEvents = new List<Event>();
            userIds.ToList().ForEach(x =>
            {
                foundedEvents.AddRange(EventsForAMonthForAUser(x, startDate));
            });
            return foundedEvents;
        }
        private IEnumerable<Event> EventsForADateBetween(int userId, DateTime startDate, DateTime? endDate)
        {
            var filters = new List<Expression<Func<Event, bool>>>();
            filters.Add(e => e.ResponsibleId == userId);
            filters.Add(x => x.EventDate >= startDate && x.EventDate <= endDate.Value);

            return uow.EventRepo.Get(filters);
        }
        private IEnumerable<Event> EventsForAMonthForAUser(int userId, DateTime startDate)
        {
            var filters = new List<Expression<Func<Event, bool>>>();
            filters.Add(e => e.ResponsibleId == userId);
            filters.Add(e => e.EventDate.Month == startDate.Month);
            filters.Add(e => e.EventDate.Year == startDate.Year);

            return uow.EventRepo.Get(filters);
        }
        private bool IsNeededEventsForAMonth(DateTime startDate, DateTime? endDate)
        {
            return !endDate.HasValue && startDate.Day == 1;
        }
    }
}
