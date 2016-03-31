using BotData.Abstract;
using BotLibrary.Entities;
using BotLibrary.Repositories;

namespace BotData.DumbData.Repositories
{
    public class CandidateRepository : BaseEntityRepository<Candidate>, ICandidateRepository
    {
        public CandidateRepository(IContext context) : base(context)
        {
            Collection = _context.Candidates;
        }
    }
}
