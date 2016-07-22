using System.ComponentModel.DataAnnotations;

namespace DAL.DTO.SetupDTO
{
    public class CountryDTO : BaseEntityDTO
    {
        [Required]
        public string Title { get; set; }
    }
}
