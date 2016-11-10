using Domain.Entities.Enum.Setup;

namespace DAL.Mapping
{
    public class SocialNetworkConfiguration : BaseEntityConfiguration<SocialNetwork>
    {
        public SocialNetworkConfiguration()
        {
            Property(sn => sn.Title).IsRequired();
            Property(sn => sn.ImagePath).IsOptional();
        }
    }
}