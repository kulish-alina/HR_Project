namespace DAL.DTO
{
    public class RoleDTO : BaseEntityDTO
    {
        public string Title { get; set; }
        public int Permissions { get; set; }
    }
}
