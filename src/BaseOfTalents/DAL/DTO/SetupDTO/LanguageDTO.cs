using System.ComponentModel.DataAnnotations;

namespace DAL.DTO.SetupDTO
{
    public class LanguageDTO : BaseEntityDTO
    {
        [Required]
        public string Title { get; set; }
    }
}
