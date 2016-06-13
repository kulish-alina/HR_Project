using BaseOfTalents.DAL.Infrastructure;
using BaseOfTalents.DAL.Repositories;
using BaseOfTalents.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class EventRepository : BaseRepository<Event>, IEventRepository
    {
        public EventRepository(DbContext context) : base(context)
        {
        }
    }
}
