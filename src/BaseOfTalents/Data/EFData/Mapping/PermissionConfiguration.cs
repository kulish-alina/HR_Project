using Domain.Entities.Setup;

namespace Data.EFData.Mapping
{
    public class PermissionConfiguration : BaseEntityConfiguration<Permission>
    {
        public PermissionConfiguration()
        {
            Property(p => p.Description).IsRequired();
            Property(p => p.AccessRights).IsRequired();
            Property(p => p.Group).IsRequired();
        }
    }
}
