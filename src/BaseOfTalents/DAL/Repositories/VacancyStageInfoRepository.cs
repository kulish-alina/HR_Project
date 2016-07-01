using DAL.Infrastructure;
using Domain.Entities;
using System.Data.Entity;

namespace DAL.Repositories
{
    public class VacancyStageInfoRepository : BaseRepository<VacancyStageInfo>, IVacancyStageInfoRepository
    {
        public VacancyStageInfoRepository(DbContext context) : base(context)
        {

        }
    }
}
