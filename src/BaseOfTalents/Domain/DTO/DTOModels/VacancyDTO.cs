using Domain.Entities;
using Domain.Entities.Enum;
using Domain.Entities.Enum.Setup;
using Domain.Entities.Setup;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Domain.DTO.DTOModels
{
    public class VacancyDTO
    {
        public int Id { get; set; }
        public DateTime EditTime { get; set; }
        public string Title { get; set; }

        public EntityState State { get; set; }
        public Industry Industry { get; set; }
        public IEnumerable<Level> Levels { get; set; }
        public string Description { get; set; }
        public int DepartmentId { get; set; }
        public IEnumerable<int> LocationIds { get; set; }
        public User Responsible { get; set; }
        public IEnumerable<int> RequiredSkillsIds { get; set; }
        public int SalaryMin { get; set; }
        public int SalaryMax { get; set; }
        public LanguageSkillDTO LanguageSkill { get; set; }
        public TypeOfEmployment TypeOfEmployment { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime DeadlineDate { get; set; }
        public IEnumerable<VacancyStageInfoDTO> CandidatesProgress { get; set; }
        public int ParentVacancyId { get; set; }
        public IEnumerable<File> Files { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
        public IEnumerable<Tag> Tags { get; set; }

    }
}