using Domain.Entities;
using Domain.Repositories;

namespace Data.DumbData.Repositories
{
    public class DummyVacancyRepository : DummyBaseEntityRepository<Vacancy>, IVacancyRepository
    {
        public DummyVacancyRepository(DummyBotContext context) : base(context)
        {
            Collection = _context.Vacancies;
        }
    }
}
