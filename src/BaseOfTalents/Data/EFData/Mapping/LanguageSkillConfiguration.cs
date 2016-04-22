using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EFData.Mapping
{
    public class LanguageSkillConfiguration : BaseEntityConfiguration<LanguageSkill>
    {
        public LanguageSkillConfiguration()
        {
            Property(ls => ls.LanguageLevel).IsRequired();
        }
    }
}
