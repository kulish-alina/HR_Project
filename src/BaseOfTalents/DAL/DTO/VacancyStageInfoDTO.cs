using Domain.Entities.Enum;
using System;

namespace DAL.DTO
{
    public class VacancyStageInfoDTO : BaseEntityDTO
    {
        public int StageId { get; set; }
        public StageState StageState { get; set; }
        public int CandidateId { get; set; }
        public int? VacancyId { get; set; }
        public CommentDTO Comment { get; set; }
        public DateTime? DateOfPass { get; set; }
    }
}
