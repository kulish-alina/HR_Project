using Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.DTOModels
{
    public class VacancyStageDTO
    {
        public int Id { get; set; }
        public DateTime EditTime { get; set; }
        public EntityState State { get; set; }

        public int VacancyId { get; set; }
        public int StageId { get; set; }
        public int Order { get; set; }
        public bool IsCommentRequired { get; set; }
    }
}
