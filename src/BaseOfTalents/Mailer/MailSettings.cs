namespace Mailer
{
    public class MailSettings
    {
        public int Timeout { get; set; }
        public int Port { get; set; }
        public string Host { get; set; }

        public string Password { get; set; }
        public string Sender { get; set; }

        public static MailSettings DefaultSettings { get; } = new MailSettings
        {
            Host = "mail.isd.dp.ua",
            Port = 25,
            Timeout = 20000
        };
    }
}
