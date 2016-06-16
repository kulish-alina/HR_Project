using BaseOfTalents.Domain.Entities.Enum.Setup;

namespace BaseOfTalents.DAL.Mapping
{
    public class SocialNetworkConfiguration : BaseEntityConfiguration<SocialNetwork>
    {
        public SocialNetworkConfiguration()
        {
            Property(sn => sn.Title).IsRequired();
            Property(sn => sn.ImagePath).IsRequired();

            HasMany(x => x.CandidateSocials).WithRequired(x => x.SocialNetwork).HasForeignKey(x => x.SocialNetworkId);
        }
    }
}