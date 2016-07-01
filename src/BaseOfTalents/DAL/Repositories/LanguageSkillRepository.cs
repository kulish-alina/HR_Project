using DAL.Infrastructure;
using Domain.Entities;
using System.Data.Entity;

namespace DAL.Repositories
{
    public class LanguageSkillRepository : BaseRepository<LanguageSkill>, ILanguageSkillRepository
    {
        public LanguageSkillRepository(DbContext context) : base(context)
        {

        }
    }
}
