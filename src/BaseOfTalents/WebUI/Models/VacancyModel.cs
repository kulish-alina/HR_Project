using BaseOfTalents.Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using WebUI.Models;

namespace BaseOfTalents.WebUI.Models
{
    public class VacancyModel
    {
        public VacancyModel()
        {
            LevelIds = new List<int>();
            LocationIds = new List<int>();
            RequiredSkillIds = new List<int>();
            TagIds = new List<int>();
            CandidatesProgress = new List<VacancyStageModel>();
            Files = new List<FileModel>();
        }

        public string Title { get; set; }
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
        public IEnumerable<VacancyStageModel> CandidatesProgress { get; set; }
        public IEnumerable<int> TagIds { get; set; }
        public IEnumerable<FileModel> Files { get; set; }

        public int? ParentVacancyId { get; set; }

        public int? IndustryId { get; set; }

        public int DepartmentId { get; set; }

        public int ResponsibleId { get; set; }

        public LanguageSkillModel LanguageSkill { get; set; }
    }
}