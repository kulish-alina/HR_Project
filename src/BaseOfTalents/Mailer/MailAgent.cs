using System;
using System.Net.Mail;

namespace Mailer
{
    public class MailAgent
    {
        private static MailSettings settings;

        /// <summary>
        /// Sets the settings that are important for sending email
        /// </summary>
        /// <param name="newSettings">Settings for smtp client initialization</param>
        public static void SetConfiguration(MailSettings newSettings)
        {
            if (newSettings == null)
            {
                throw new ArgumentException("Settings can't be null");
            }
            settings = newSettings;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Instance of default default mail settings <see cref="MailSettings.DefaultSettings"/></returns>
        public static MailSettings UseDefaultConfiguration()
        {
            return settings = MailSettings.DefaultSettings;
        }

        /// <summary>
        /// Sends email onto specified mail address
        /// </summary>
        /// <param name="to">Email of user to send email to</param>
        /// <param name="subject">Subject of the letter</param>
        /// <param name="content">Content of the letter</param>
        public static void Send(string to, string subject, string content)
        {
            var mail = new MailMessage(settings.Sender, to)
            {
                Subject = subject,
                IsBodyHtml = true,
                Body = content
            };

            Send(mail);
        }

        /// <summary>
        /// Sends specified mail message
        /// </summary>
        /// <param name="message">The message that is needed to be send to the user</param>
        public static void Send(MailMessage message)
        {
            var client = new SmtpClient(settings.Host, settings.Port);
            client.SendCompleted += (s, e) =>
            {
                client.Dispose();
                message.Dispose();
            };
            client?.SendAsync(message, null);
        }
    }
}