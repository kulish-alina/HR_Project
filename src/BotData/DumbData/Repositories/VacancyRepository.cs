using BotData.Abstract;
using BotLibrary.Entities;
using BotLibrary.Repositories;

namespace BotData.DumbData.Repositories
{
    public class VacancyRepository : BaseEntityRepository<Vacancy>, IVacancyRepository
    {
        public VacancyRepository(IContext context) : base(context)
        {
            Collection = _context.Vacancies;
        }
    }
}
