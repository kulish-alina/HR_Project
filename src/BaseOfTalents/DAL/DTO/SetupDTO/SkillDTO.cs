using System.ComponentModel.DataAnnotations;

namespace DAL.DTO.SetupDTO
{
    public class SkillDTO : BaseEntityDTO
    {
        [Required]
        public string Title { get; set; }
    }
}
