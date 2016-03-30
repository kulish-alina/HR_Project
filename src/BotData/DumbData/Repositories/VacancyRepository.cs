using BotLibrary.Entities;
using BotLibrary.Repositories;

namespace BotData.DumbData.Repositories
{
    public class VacancyRepository : BaseEntityRepository<Vacancy>, IVacancyRepository
    {
        public VacancyRepository()
        {
            Collection = DummyBotContext.Context.Vacancies;
        }
    }
}
