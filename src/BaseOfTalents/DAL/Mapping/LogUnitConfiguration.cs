using Domain.Entities;

namespace DAL.Mapping
{
    public class LogUnitConfiguration : BaseEntityConfiguration<LogUnit>
    {
        public LogUnitConfiguration()
        {
            HasRequired(x => x.User).WithMany().HasForeignKey(x => x.UserId);
            HasMany(x => x.Values).WithMany().Map(x =>
             {
                 x.MapLeftKey("LogValueId");
                 x.MapRightKey("LogUnitId");
                 x.ToTable("LogUnitToLogValue");
             });
        }
    }
}
