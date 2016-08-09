using Domain.Entities.Enum.Setup;

namespace DAL.Mapping
{
    class ExtendedStageConfiguration : BaseEntityConfiguration<ExtendedStage>
    {
        public ExtendedStageConfiguration()
        {
            HasRequired(x => x.Stage).WithMany().HasForeignKey(x => x.StageId);

            Property(sn => sn.Order).IsRequired();
        }
    }
}
