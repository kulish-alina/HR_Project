using Domain.Entities.Enum.Setup;

namespace Domain.Entities
{
    public class CandidateSocial : BaseEntity
    {
        public string Path { get; set; }

        public int SocialNetworkId { get; set; }
        public virtual SocialNetwork SocialNetwork { get; set; }
    }
}