using BaseOfTalents.Domain.Entities.Enum.Setup;

namespace BaseOfTalents.DAL.Mapping
{
    public class DepartmentGroupConfiguration : BaseEntityConfiguration<DepartmentGroup>
    {
        public DepartmentGroupConfiguration()
        {
            Property(dg => dg.Title).IsRequired();
        }
    }
}