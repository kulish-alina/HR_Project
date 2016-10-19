namespace Domain.Entities
{
    public class MailContent : BaseEntity
    {
        public string Subject { get; set; }
        public string Invitation { get; set; }
        public string Body { get; set; }
        public string Farewell { get; set; }
    }
}
