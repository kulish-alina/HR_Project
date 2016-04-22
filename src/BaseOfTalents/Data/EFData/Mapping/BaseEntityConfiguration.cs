using System.Data.Entity.ModelConfiguration;using Domain.Entities;

namespace Data.EFData.Mapping
{
    public class BaseEntityConfiguration<T> : EntityTypeConfiguration<T> where T : BaseEntity
    {
        public BaseEntityConfiguration()
        {
            HasKey(e => e.Id);
            Property(e => e.EditTime).IsRequired();
            Property(e => e.State).IsRequired();
        }
    }
}
