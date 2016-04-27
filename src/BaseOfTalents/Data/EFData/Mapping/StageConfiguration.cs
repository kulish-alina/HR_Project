using Domain.Entities.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EFData.Mapping
{
    public class StageConfiguration : BaseEntityConfiguration<Stage>
    {
        public StageConfiguration()
        {
            Property(sn => sn.Title).IsRequired();
        }
    }
}
