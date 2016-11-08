using System;

namespace DAL.DTO.ReportDTO
{
    public class StageInfoDTO
    {
        public int StageId { get; set; }
        public string StageTitle { get; set; }
        public string Comment { get; set; }
        public DateTime PassDate { get; set; }
    }
}
