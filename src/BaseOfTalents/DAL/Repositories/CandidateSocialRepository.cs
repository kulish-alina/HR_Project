using DAL.Infrastructure;
using Domain.Entities;
using System.Data.Entity;

namespace DAL.Repositories
{
    public class CandidateSocialRepository : BaseRepository<CandidateSocial>, ICandidateSocialRepository
    {
        public CandidateSocialRepository(DbContext context) : base(context)
        {

        }
    }
}
