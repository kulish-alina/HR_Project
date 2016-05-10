using System.Data.Entity.ModelConfiguration;
using Domain.Entities;

namespace Data.EFData.Mapping
{
    public class BaseEntityConfiguration<T> : EntityTypeConfiguration<T> where T : BaseEntity
    {
        public BaseEntityConfiguration()
        {
            Map(m => m.Requires("IsDeleted").HasValue(false)).Ignore(m => m.IsDeleted);

            HasKey(e => e.Id);
            Property(e => e.EditTime);
            Property(e => e.State).IsRequired();
        }
    }
}
