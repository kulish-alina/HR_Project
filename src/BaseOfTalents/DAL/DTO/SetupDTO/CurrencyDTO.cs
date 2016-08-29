using System.ComponentModel.DataAnnotations;

namespace DAL.DTO.SetupDTO
{
    public class CurrencyDTO : BaseEntityDTO
    {
        [Required]
        public string Title { get; set; }
    }
}
