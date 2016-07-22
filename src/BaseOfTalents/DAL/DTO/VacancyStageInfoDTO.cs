using System.ComponentModel.DataAnnotations;

namespace DAL.DTO
{
    public class VacancyStageInfoDTO : BaseEntityDTO
    {
        [Required]
        public int StageId { get; set; }
        public bool IsPassed { get; set; }
        public int CandidateId { get; set; }
        public int? VacancyId { get; set; }
        public CommentDTO Comment { get; set; }
    }
}
