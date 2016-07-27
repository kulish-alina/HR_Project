using Domain.Entities.Enum.Setup;

namespace DAL.Mapping
{
    public class StageConfiguration : BaseEntityConfiguration<Stage>
    {
        public StageConfiguration()
        {
            Property(sn => sn.Title).IsRequired();
            Property(sn => sn.IsCommentRequired).IsRequired();
            Property(sn => sn.IsDefault).IsRequired();
            Property(sn => sn.Order).IsRequired();
        }
    }
}