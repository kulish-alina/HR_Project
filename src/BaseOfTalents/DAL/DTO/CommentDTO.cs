namespace DAL.DTO
{
    public class CommentDTO : BaseEntityDTO
    {
        public string Message { get; set; }
        public int AuthorId { get; set; }
    }
}
