using BotLibrary.Entities;
using BotLibrary.Repositories;

namespace BotData.DumbData.Repositories
{
    public class CandidateRepository : BaseEntityRepository<Candidate>, ICandidateRepository
    {
        public CandidateRepository()
        {
            Collection = DummyBotContext.Context.Candidates;
        }
    }
}
