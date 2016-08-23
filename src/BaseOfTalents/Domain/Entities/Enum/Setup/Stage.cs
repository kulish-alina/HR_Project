namespace Domain.Entities.Enum.Setup
{
    public class Stage : BaseEntity
    {
        public string Title { get; set; }
        public int Order { get; set; }
        public bool IsDefault { get; set; }
        public bool IsCommentRequired { get; set; }
        public StageType StageType { get; set; }
    }
}