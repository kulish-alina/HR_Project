using BaseOfTalents.DAL.Infrastructure;
using BaseOfTalents.Domain.Entities;
using System.Data.Entity;

namespace BaseOfTalents.DAL.Repositories
{
    public class CandidateRepository : BaseRepository<Candidate>, ICandidateRepository
    {
        public CandidateRepository(DbContext context) : base(context)
        {
        }
    }
}