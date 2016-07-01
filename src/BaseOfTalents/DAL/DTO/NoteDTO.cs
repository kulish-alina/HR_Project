namespace DAL.DTO
{
    public class NoteDTO : BaseEntityDTO
    {
        public string Message { get; set; }
        public int UserId { get; set; }
    }
}
