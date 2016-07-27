using System.ComponentModel.DataAnnotations;

namespace DAL.DTO.SetupDTO
{
    public class DepartmentGroupDTO : BaseEntityDTO
    {
        [Required]
        public string Title { get; set; }
    }
}
