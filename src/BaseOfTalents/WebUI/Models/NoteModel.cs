using DAL.DTO;
using Domain.Entities;

namespace BaseOfTalents.WebUI.Models
{
    public class NoteModel
    {
        public NoteDTO Note { get; set; }
        public int UserId { get; set; }
    }
}