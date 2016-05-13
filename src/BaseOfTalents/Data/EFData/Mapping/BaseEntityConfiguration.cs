using System.Data.Entity.ModelConfiguration;
using Domain.Entities;

namespace Data.EFData.Mapping
{
    public class BaseEntityConfiguration<T> : EntityTypeConfiguration<T> where T : BaseEntity
    {
        public BaseEntityConfiguration()
        {
            HasKey(e => e.Id);
            Property(e => e.CreatedOn);
            Property(e => e.LastModified);
            Property(e => e.State).IsRequired();
        }
    }
}
