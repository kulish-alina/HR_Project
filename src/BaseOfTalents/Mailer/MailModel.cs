namespace Mailer
{
    public class MailModel
    {
        public MailModel(string template)
        {
            Template = template;
        }

        public MailModel(string template, string subject)
        {
            Template = template;
            Subject = subject;
        }

        public string Subject { get; set; }
        public string Template { get; private set; }
    }
}