using BaseOfTalents.Domain.Entities.Enum.Setup;

namespace BaseOfTalents.DAL.Mapping
{
    public class StageConfiguration : BaseEntityConfiguration<Stage>
    {
        public StageConfiguration()
        {
            Property(sn => sn.Title).IsRequired();
        }
    }
}