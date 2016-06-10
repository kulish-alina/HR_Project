using BaseOfTalents.Domain.Entities.Enum.Setup;

namespace BaseOfTalents.Domain.Entities
{
    public class VacancyStage : BaseEntity
    {
        public int Order { get; set; }
        public bool IsCommentRequired { get; set; }

        public int StageId { get; set; }
        public virtual Stage Stage { get; set; }
    }
}