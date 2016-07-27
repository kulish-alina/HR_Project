using Domain.Entities.Enum.Setup;

namespace DAL.Mapping
{
    public class DepartmentConfiguration : BaseEntityConfiguration<Department>
    {
        public DepartmentConfiguration()
        {
            Property(d => d.Title).IsRequired();
        }
    }
}