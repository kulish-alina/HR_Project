using BaseOfTalents.DAL.Infrastructure;
using BaseOfTalents.Domain.Entities.Enum.Setup;
using System.Data.Entity;

namespace BaseOfTalents.DAL.Repositories
{
    public class SkillRepository : BaseRepository<Skill>, ISkillRepository
    {
        public SkillRepository(DbContext context) : base(context)
        {
        }
    }
}