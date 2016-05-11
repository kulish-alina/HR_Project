using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EFData.Mapping
{
    public class FileConfiguration : BaseEntityConfiguration<File>
    {
        public FileConfiguration()
        {
            Property(f => f.FilePath).IsRequired();
            Property(f => f.Description).IsOptional();
        }
    }
}
