using DAL.DTO;
using Mailer;

namespace WebUI.Extensions
{
    public static class UserMailExtension
    {
        public static UserDTO SendEmail(this UserDTO user, MailModel mail)
        {
            MailAgent.Send(user.Email, mail.Subject, mail.Template);
            return user;
        }
    }
}