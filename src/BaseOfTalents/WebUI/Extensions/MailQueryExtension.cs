using Mailer;

namespace WebUI.Extensions
{
    public static class MailQueryExtension
    {
        public static MailSettings UseTimeout(this MailSettings settings, int timeout)
        {
            settings.Timeout = timeout;
            return settings;
        }

        public static MailSettings UseHost(this MailSettings settings, string host)
        {
            settings.Host = host;
            return settings;
        }

        public static MailSettings UsePort(this MailSettings settings, int port)
        {
            settings.Port = port;
            return settings;
        }

        public static MailSettings UsePassword(this MailSettings settings, string password)
        {
            settings.Password = password;
            return settings;
        }

        public static MailSettings UseSender(this MailSettings settings, string sender)
        {
            settings.Sender = sender;
            return settings;
        }
    }
}