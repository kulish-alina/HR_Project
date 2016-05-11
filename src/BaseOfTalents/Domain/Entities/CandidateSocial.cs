using Domain.Entities.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class CandidateSocial : BaseEntity
    {
        public string Path { get; set; }

        public int SocialNetworkId { get; set; }
        public virtual SocialNetwork SocialNetwork { get; set; }

    }
}
