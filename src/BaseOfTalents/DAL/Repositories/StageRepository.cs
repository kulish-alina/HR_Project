using BaseOfTalents.DAL.Infrastructure;
using BaseOfTalents.DAL.Repositories;
using BaseOfTalents.Domain.Entities.Enum.Setup;
using System.Data.Entity;

namespace DAL.Repositories
{
    public class StageRepository : BaseRepository<Stage>, IStageRepository
    {
        public StageRepository(DbContext context) : base(context)
        {

        }
    }
}
