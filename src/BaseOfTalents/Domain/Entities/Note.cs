namespace Domain.Entities
{
    public class Note : BaseEntity
    {
        public string Message { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
