using Domain.Entities.Enum;
using Domain.Entities.Setup;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Vacancy : BaseEntity 
    {
        public string Title { get; set; }
        public Level Level {get; set;}
        public string Description { get; set; }
        public Team Team { get; set; }
        public virtual City City { get; set; }
        public User Responsible { get; set; }
        public List<Skill> RequiredSkills { get; set; }
        public int SalaryMin { get; set; }
        public int SalaryMax { get; set; }
        public LanguageSkill LanguageSkill { get; set; }
        public TypeOfEmployment TypeOfEmployment { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime DeadlineDate { get; set; }
        public List<VacancyStageInfo> CandidatesProgress { get; set; }
        public Vacancy ParentVacancy { get; set; }
        public List<File> Files { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
