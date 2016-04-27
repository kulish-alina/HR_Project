using Domain.Entities.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EFData.Mapping
{
    public class EventTypeConfiguration : BaseEntityConfiguration<EventType>
    {
        public EventTypeConfiguration()
        {
            Property(et => et.ImagePath).IsRequired();
            Property(et => et.Title).IsRequired();
        }
    }
}
