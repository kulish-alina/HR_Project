namespace Domain.Entities
{
    public class File : BaseEntity
    {
        public string FilePath { get; set; }
        public string Description { get; set; }
        public long Size { get; set; }
    }
}