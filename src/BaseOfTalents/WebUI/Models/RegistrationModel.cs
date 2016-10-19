using DAL.DTO;

namespace WebUI.Models
{
    public class RegistrationModel : UserDTO
    {
        public int MailId { get; set; }
    }
}