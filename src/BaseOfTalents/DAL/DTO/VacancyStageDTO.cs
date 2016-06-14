using Domain.Entities.Enum;
using System;
namespace Domain.DTO.DTOModels
{
    public class VacancyStageDTO : BaseEntityDTO
    {
        public int StageId { get; set; }
        public int Order { get; set; }
        public bool IsCommentRequired { get; set; }
    }
}
