using BaseOfTalents.Domain.Entities;

namespace BaseOfTalents.DAL.Mapping
{
    public class CandidateSocialConfiguration : BaseEntityConfiguration<CandidateSocial>
    {
        public CandidateSocialConfiguration()
        {
            Property(cs => cs.Path).IsRequired();

            HasRequired(cs => cs.SocialNetwork)
                .WithMany(x => x.CandidateSocials)
                .HasForeignKey(cs => cs.SocialNetworkId);
        }
    }
}