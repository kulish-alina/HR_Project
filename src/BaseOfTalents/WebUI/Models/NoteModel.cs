using DAL.DTO;

namespace WebUI.Models
{
    public class NoteModel
    {
        public NoteDTO Note { get; set; }
        public int UserId { get; set; }
    }
}