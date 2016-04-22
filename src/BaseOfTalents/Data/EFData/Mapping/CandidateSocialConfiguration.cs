using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EFData.Mapping
{
    public class CandidateSocialConfiguration : BaseEntityConfiguration<CandidateSocial>
    {
        public CandidateSocialConfiguration()
        {
            Property(cs => cs.Path).IsRequired();
            HasRequired(cs => cs.SocialNetwork).WithMany().HasForeignKey(cs=>cs.SocialNetworkId);
        }
    }
}
