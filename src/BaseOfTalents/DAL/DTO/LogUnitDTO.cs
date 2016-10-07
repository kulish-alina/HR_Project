namespace DAL.DTO
{
    public class LogUnitDTO : BaseEntityDTO
    {
        public int UserId { get; set; }
        public string Field { get; set; }
        public string Value { get; set; }
    }
}
