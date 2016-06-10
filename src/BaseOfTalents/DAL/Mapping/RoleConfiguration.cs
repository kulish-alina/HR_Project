using BaseOfTalents.Domain.Entities.Enum.Setup;

namespace BaseOfTalents.DAL.Mapping
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