using System.ComponentModel.DataAnnotations;

namespace DAL.DTO.SetupDTO
{
    public class TagDTO : BaseEntityDTO
    {
        [Required]
        public string Title { get; set; }
    }
}
