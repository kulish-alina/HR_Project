using Domain.Entities.Enum.Setup;

namespace DAL.Mapping
{
    internal class RoleConfiguration : BaseEntityConfiguration<Role>
    {
        public RoleConfiguration()
        {
            Property(r => r.Title);
            HasMany(r => r.Permissions).WithMany(p => p.Roles);
        }
    }
}