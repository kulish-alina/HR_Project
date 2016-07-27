using System.ComponentModel.DataAnnotations;

namespace DAL.DTO.SetupDTO
{
    public class CityDTO : BaseEntityDTO
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public int CountryId { get; set; }
    }
}
