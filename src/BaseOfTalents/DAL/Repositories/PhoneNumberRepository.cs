using DAL.Infrastructure;
using Domain.Entities;
using System.Data.Entity;

namespace DAL.Repositories
{
    public class PhoneNumberRepository : BaseRepository<PhoneNumber>, IPhoneNumberRepository
    {
        public PhoneNumberRepository(DbContext context) : base(context)
        {

        }
    }
}
