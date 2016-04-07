using Domain.Entities;
using Domain.Repositories;

namespace Data.DumbData.Repositories
{
    public class DummyCandidateRepository : DummyBaseEntityRepository<Candidate>, ICandidateRepository
    {
        public DummyCandidateRepository(DummyBotContext context) : base(context)
        {
            Collection = _context.Candidates;
        }
    }
}
