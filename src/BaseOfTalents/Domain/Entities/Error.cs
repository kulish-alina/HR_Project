namespace BaseOfTalents.Domain.Entities
{
    public class Error : BaseEntity
    {
        public string Message { get; set; }
        public string StackTrace { get; set; }
    }
}