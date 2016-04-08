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
        public virtual Team Team { get; set; }
        public virtual City City { get; set; }
        public virtual User Responsible { get; set; }
        public virtual List<Skill> RequiredSkills { get; set; }
        public int SalaryMin { get; set; }
        public int SalaryMax { get; set; }
        public virtual LanguageSkill LanguageSkill { get; set; }
        public TypeOfEmployment TypeOfEmployment { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime DeadlineDate { get; set; }
        public virtual List<VacancyStageInfo> CandidatesProgress { get; set; }
        public virtual Vacancy ParentVacancy { get; set; }
        public virtual List<File> Files { get; set; }
        public virtual List<Comment> Comments { get; set; }
    }
}
