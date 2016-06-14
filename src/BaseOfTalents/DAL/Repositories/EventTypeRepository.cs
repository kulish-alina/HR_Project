using BaseOfTalents.DAL.Infrastructure;
using BaseOfTalents.DAL.Repositories;
using BaseOfTalents.Domain.Entities.Enum.Setup;
using System.Data.Entity;

namespace DAL.Repositories
{
    public class EventTypeRepository : BaseRepository<EventType>, IEventTypeRepository
    {
        public EventTypeRepository(DbContext context) : base(context)
        {

        }
    }
}
