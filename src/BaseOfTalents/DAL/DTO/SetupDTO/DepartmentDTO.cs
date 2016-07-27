using System.ComponentModel.DataAnnotations;

namespace DAL.DTO.SetupDTO
{
    public class DepartmentDTO : BaseEntityDTO
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public int DepartmentGroupId { get; set; }
    }
}

