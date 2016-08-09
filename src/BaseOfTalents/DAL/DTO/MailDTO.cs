namespace DAL.DTO
{
    public class MailDTO : BaseEntityDTO
    {
        public string Subject { get; set; }
        public string Invitation { get; set; }
        public string Body { get; set; }
        public string Farewell { get; set; }
    }
}
