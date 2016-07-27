using Domain.Entities;

namespace DAL.Mapping
{
    public class CandidateSocialConfiguration : BaseEntityConfiguration<CandidateSocial>
    {
        public CandidateSocialConfiguration()
        {
            Property(cs => cs.Path).IsRequired();

            HasRequired(cs => cs.SocialNetwork)
                .WithMany()
                .HasForeignKey(cs => cs.SocialNetworkId);
        }
    }
}