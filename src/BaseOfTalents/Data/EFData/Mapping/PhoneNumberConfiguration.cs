using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EFData.Mapping
{
    public class PhoneNumberConfiguration : BaseEntityConfiguration<PhoneNumber>
    {
        public PhoneNumberConfiguration()
        {
            Property(p => p.Number).IsRequired();
        }
    }
}
