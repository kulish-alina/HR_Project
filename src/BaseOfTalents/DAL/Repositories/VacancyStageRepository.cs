using BaseOfTalents.DAL.Repositories;
using BaseOfTalents.Domain.Entities;
using DAL.Infrastructure;
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
