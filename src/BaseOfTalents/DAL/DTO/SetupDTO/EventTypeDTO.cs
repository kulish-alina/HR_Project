using System.ComponentModel.DataAnnotations;

namespace DAL.DTO.SetupDTO
{
    public class EventTypeDTO : BaseEntityDTO
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string ImagePath { get; set; }
    }
}
