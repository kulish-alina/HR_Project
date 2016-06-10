using BaseOfTalents.DAL.Infrastructure;
using BaseOfTalents.Domain.Entities.Enum.Setup;
using System.Data.Entity;

namespace BaseOfTalents.DAL.Repositories
{
    public class SocialNetworkRepository : BaseRepository<SocialNetwork>, ISocialNetworkRepository
    {
        public SocialNetworkRepository(DbContext context) : base(context)
        {

        }
    }
}