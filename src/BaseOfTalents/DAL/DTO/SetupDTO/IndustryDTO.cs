using System.ComponentModel.DataAnnotations;

namespace DAL.DTO.SetupDTO
{
    public class IndustryDTO : BaseEntityDTO
    {
        [Required]
        public string Title { get; set; }
    }
}
