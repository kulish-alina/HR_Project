using Mailer;

namespace WebUI.Extensions
{
    public class MailAgentExtension
    {
        public static void Send(string email, MailModel mail)
        {
            MailAgent.Send(email, mail.Subject, mail.Template);
        }
    }
}