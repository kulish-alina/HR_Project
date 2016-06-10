namespace BaseOfTalents.Domain.Entities
{
    public class Photo : BaseEntity
    {
        public string ImagePath { get; set; }
        public string Description { get; set; }
    }
}