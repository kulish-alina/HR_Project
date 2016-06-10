using BaseOfTalents.DAL.Repositories;
using BaseOfTalents.Domain.Entities;
using DAL.Infrastructure;
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
