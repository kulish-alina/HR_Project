using Domain.Entities.Enum.Setup;

namespace DAL.Mapping
{
    public class PermissionConfiguration : BaseEntityConfiguration<Permission>
    {
        public PermissionConfiguration()
        {
            Property(p => p.Description).IsRequired();
            Property(p => p.AccessRights).IsRequired();
            Property(p => p.Group).IsRequired();

            HasMany(p => p.Roles).WithMany(r => r.Permissions);
        }
    }
}