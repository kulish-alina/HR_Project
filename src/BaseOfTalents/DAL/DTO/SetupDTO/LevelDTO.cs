using System.ComponentModel.DataAnnotations;

namespace DAL.DTO.SetupDTO
{
    public class LevelDTO : BaseEntityDTO
    {
        [Required]
        public string Title { get; set; }
    }
}
