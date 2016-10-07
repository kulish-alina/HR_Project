namespace Domain.Entities
{
    public class LogUnit : BaseEntity
    {
        public virtual User User { get; set; }
        public int UserId { get; set; }
        public string Field { get; set; }
        public string Value { get; set; }
    }
}
