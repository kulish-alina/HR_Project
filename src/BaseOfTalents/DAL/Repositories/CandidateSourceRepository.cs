using DAL.Infrastructure;
using Domain.Entities;
using System.Data.Entity;

namespace DAL.Repositories
{
    public class CandidateSourceRepository : BaseRepository<CandidateSource>, ICandidateSourceRepository
    {
        public CandidateSourceRepository(DbContext context) : base(context)
        {

        }
    }
}
