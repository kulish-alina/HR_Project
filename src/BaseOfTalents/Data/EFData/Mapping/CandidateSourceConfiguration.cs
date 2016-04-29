using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EFData.Mapping
{
    public class CandidateSourceConfiguration : BaseEntityConfiguration<CandidateSource>
    {
        public CandidateSourceConfiguration()
        {
            Property(sn => sn.Source).IsRequired();
            Property(sn => sn.Path).IsRequired();
        }
    }
}
