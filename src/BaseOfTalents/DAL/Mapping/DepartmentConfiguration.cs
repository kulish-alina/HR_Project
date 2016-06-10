using BaseOfTalents.Domain.Entities.Enum.Setup;

namespace BaseOfTalents.DAL.Mapping
{
    public class DepartmentConfiguration : BaseEntityConfiguration<Department>
    {
        public DepartmentConfiguration()
        {
            Property(d => d.Title).IsRequired();
        }
    }
}