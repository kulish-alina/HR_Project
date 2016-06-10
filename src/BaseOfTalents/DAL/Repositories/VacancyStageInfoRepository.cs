using BaseOfTalents.DAL.Repositories;
using BaseOfTalents.Domain.Entities;
using DAL.Infrastructure;
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
