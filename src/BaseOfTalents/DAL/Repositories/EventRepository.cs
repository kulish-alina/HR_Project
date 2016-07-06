using DAL.Infrastructure;
using Domain.Entities;
using System.Data.Entity;

namespace DAL.Repositories
{
    public class EventRepository : BaseRepository<Event>, IEventRepository
    {
        public EventRepository(DbContext context) : base(context)
        {
        }
    }
}
