using Domain.Entities.Enum.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EFData.Mapping
{
    public class IndustryConfiguration : BaseEntityConfiguration<Industry>
    {
        public IndustryConfiguration()
        {
            Property(i => i.Title).IsRequired();
        }
    }
}
