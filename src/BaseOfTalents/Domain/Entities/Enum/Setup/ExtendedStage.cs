namespace Domain.Entities.Enum.Setup
{
    public class ExtendedStage : BaseEntity
    {
        public virtual Stage Stage { get; set; }
        public int StageId { get; set; }

        public int Order { get; set; }
    }
}
