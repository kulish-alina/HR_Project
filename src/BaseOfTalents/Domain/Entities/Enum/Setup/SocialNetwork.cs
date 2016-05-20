using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Setup
{
    public class SocialNetwork: BaseEntity
    {
        public string Title { get; set; }
        public string ImagePath { get; set; }

        public virtual ICollection<CandidateSocial> CandidateSocials { get; set; }

        public SocialNetwork()
        {
            CandidateSocials = new List<CandidateSocial>();
        }
    }
}
