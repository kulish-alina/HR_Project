using DAL.DTO.SetupDTO;
using Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.DTO
{
    public class VacancyDTO : BaseEntityDTO
    {
        public VacancyDTO()
        {
            LevelIds = new List<int>();
            CityIds = new List<int>();
            RequiredSkillIds = new List<int>();
            TagIds = new List<int>();
            CandidatesProgress = new List<VacancyStageInfoDTO>();
            Files = new List<FileDTO>();
            Comments = new List<CommentDTO>();
            StageFlow = new List<ExtendedStageDTO>();
            StatesInfo = new List<VacancyStateDTO>();
        }

        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public TypeOfEmployment? TypeOfEmployment { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? DeadlineDate { get; set; }
        public bool DeadlineToCalendar { get; set; }

        public IEnumerable<int> LevelIds { get; set; }
        public IEnumerable<int> CityIds { get; set; }
        public IEnumerable<int> RequiredSkillIds { get; set; }
        public IEnumerable<ExtendedStageDTO> StageFlow { get; set; }
        public IEnumerable<VacancyStageInfoDTO> CandidatesProgress { get; set; }
        public IEnumerable<int> TagIds { get; set; }
        public IEnumerable<FileDTO> Files { get; set; }
        public IEnumerable<CommentDTO> Comments { get; set; }
        public IEnumerable<VacancyStateDTO> StatesInfo { get; set; }
        public IEnumerable<LogUnitDTO> History { get; set; }

        public int? ClosingCandidateId { get; set; }
        public int? ParentVacancyId { get; set; }
        public int? CloneVacanciesNumber { get; set; }

        public int? SalaryMin { get; set; }
        public int? SalaryMax { get; set; }

        public int? CurrencyId { get; set; }
        [Required]
        public int IndustryId { get; set; }
        [Required]
        public int DepartmentId { get; set; }
        [Required]
        public int ResponsibleId { get; set; }

        public LanguageSkillDTO LanguageSkill { get; set; }

        public bool HasParent()
        {
            return ParentVacancyId.HasValue;
        }

    }

}