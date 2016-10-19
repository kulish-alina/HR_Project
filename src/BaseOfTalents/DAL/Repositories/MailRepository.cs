using System.Data.Entity;
using DAL.Infrastructure;
using Domain.Entities;

namespace DAL.Repositories
{
    public class MailRepository : BaseRepository<MailContent>, IRepository<MailContent>
    {
        public MailRepository(DbContext context) : base(context)
        {
        }
    }
}
