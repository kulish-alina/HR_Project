using System.ComponentModel.DataAnnotations;

namespace DAL.DTO.SetupDTO
{
    public class SourceDTO : BaseEntityDTO
    {
        [Required]
        public string Title { get; set; }
    }
}
