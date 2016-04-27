using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EFData.Mapping
{
    public class PhotoConfiguration : BaseEntityConfiguration<Photo>
    {
        public PhotoConfiguration()
        {
            Property(p => p.ImagePath).IsRequired();
            Property(p => p.Description);
        }
    }
}
