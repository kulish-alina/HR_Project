using Domain.Entities.Enum;
using System.ComponentModel.DataAnnotations;

namespace DAL.DTO.SetupDTO
{
    public class StageDTO : BaseEntityDTO
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public int Order { get; set; }
        [Required]
        public bool IsDefault { get; set; }
        [Required]
        public bool IsCommentRequired { get; set; }
        [Required]
        public StageType StageType { get; set; }
    }
}
