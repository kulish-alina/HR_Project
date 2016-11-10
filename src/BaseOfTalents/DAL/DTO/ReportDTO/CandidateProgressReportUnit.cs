using System.Collections.Generic;

namespace DAL.DTO.ReportDTO
{
    public class CandidateProgressReportUnitDTO
    {
        public int CandidateId { get; set; }
        public string CandidateFirstName { get; set; }
        public string CandidateLastName { get; set; }
        public int VacancyId { get; set; }
        public string VacancyTitle { get; set; }
        public int LocationId { get; set; }
        public IEnumerable<StageInfoDTO> Stages { get; set; }
    }
}
