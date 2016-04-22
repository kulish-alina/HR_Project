using Domain.Entities;
using Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.DTOModels
{
    public class VacancyStageInfoDTO
    {
        public int Id { get; set; }
        public DateTime EditTime { get; set; }
        public EntityState State { get; set; }

        public VacancyStageDTO VacancyStage { get; set; }
        public int CandidateId { get; set; }
        public Comment Comment { get; set; }
    }
}
