using Domain.Entities;

namespace DAL.Mapping
{
    public class LogUnitConfiguration : BaseEntityConfiguration<LogUnit>
    {
        public LogUnitConfiguration()
        {
            HasRequired(x => x.User).WithMany().HasForeignKey(x => x.UserId);
            HasMany(x => x.NewValues).WithMany().Map(x =>
             {
                 x.MapLeftKey("NewLogValueId");
                 x.MapRightKey("LogUnitId");
                 x.ToTable("LogUnitToNewLogValue");
             });
            HasMany(x => x.PastValues).WithMany().Map(x =>
            {
                x.MapLeftKey("PastLogValueId");
                x.MapRightKey("LogUnitId");
                x.ToTable("LogUnitToPastLogValue");
            });
        }
    }
}
