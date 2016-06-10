using BaseOfTalents.Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.DTO.DTOModels
{
    public class VacancyDTO : BaseEntityDTO
    {
        public VacancyDTO()
        {
            LevelIds = new List<int>();
            LocationIds = new List<int>();
            RequiredSkillIds = new List<int>();
            TagIds = new List<int>();
            CandidatesProgress = new List<VacancyStageInfoDTO>();
            Files = new List<FileDTO>();
        }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }
        public int SalaryMin { get; set; }
        public int SalaryMax { get; set; }
        public TypeOfEmployment? TypeOfEmployment { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? DeadlineDate { get; set; }

        public IEnumerable<int> LevelIds { get; set; }
        public IEnumerable<int> LocationIds { get; set; }
        public IEnumerable<int> RequiredSkillIds { get; set; }
        public IEnumerable<VacancyStageInfoDTO> CandidatesProgress { get; set; }
        public IEnumerable<int> TagIds { get; set; }
        public IEnumerable<FileDTO> Files { get; set; }
        public IEnumerable<CommentDTO> Comments { get; set; }

        public int? ParentVacancyId { get; set; }

        public int? IndustryId { get; set; }

        public int DepartmentId { get; set; }

        public int ResponsibleId { get; set; }

        public LanguageSkillDTO LanguageSkill { get; set; }
    }
}