using DAL.Infrastructure;
using Domain.Entities;
using System.Data.Entity;

namespace DAL.Repositories
{
    public class VacancyStageRepository : BaseRepository<VacancyStage>, IVacancyStageRepository
    {
        public VacancyStageRepository(DbContext context) : base(context)
        {

        }
    }
}
