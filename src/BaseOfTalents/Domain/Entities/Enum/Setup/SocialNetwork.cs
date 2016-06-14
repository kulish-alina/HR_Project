using System.Collections.Generic;

namespace BaseOfTalents.Domain.Entities.Enum.Setup
{
    public class SocialNetwork : BaseEntity
    {
        public SocialNetwork()
        {
            CandidateSocials = new List<CandidateSocial>();
        }

        public string Title { get; set; }
        public string ImagePath { get; set; }

        public virtual ICollection<CandidateSocial> CandidateSocials { get; set; }
    }
}