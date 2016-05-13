namespace Domain.DTO.DTOModels
{
    public class VacancyStageInfoDTO : BaseEntityDTO
    {
        public VacancyStageDTO VacancyStage { get; set; }
        public int CandidateId { get; set; }
        public int VacancyId { get; set; }
        public CommentDTO Comment { get; set; }
    }
}
