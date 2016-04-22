using Domain.Entities.Setup;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DumbData.Repositories
{
    public class DummySkillRepository : DummyBaseEntityRepository<Skill>, ISkillRepository
    {
        public DummySkillRepository(DummyBotContext context) : base(context)
        {
            Collection = _context.Skills;
        }

    }
}
