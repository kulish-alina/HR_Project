using Domain.Entities.Enum.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EFData.Mapping
{
    public class TagConfiguration : BaseEntityConfiguration<Tag>
    {
        public TagConfiguration()
        {
            Property(t => t.Title).IsRequired();
        }
    }
}
