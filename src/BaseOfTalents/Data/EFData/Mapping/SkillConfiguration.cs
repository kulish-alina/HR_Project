using Domain.Entities.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EFData.Mapping
{
    public class SkillConfiguration : BaseEntityConfiguration<Skill>
    {
        public SkillConfiguration()
        {
            Property(s => s.Title);
        }
    }
}
