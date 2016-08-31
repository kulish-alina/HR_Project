namespace Domain.Entities
{
    public class Comment : BaseEntity
    {
        public string Message { get; set; }

        public int AuthorId { get; set; }
        public virtual User Author { get; set; }
    }
}