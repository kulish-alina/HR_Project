using Domain.Entities.Enum.Setup;

namespace DAL.Mapping
{
    public class DepartmentGroupConfiguration : BaseEntityConfiguration<DepartmentGroup>
    {
        public DepartmentGroupConfiguration()
        {
            Property(dg => dg.Title).IsRequired();
        }
    }
}