using Domain.Entities.Enum;
using Domain.Entities.Enum.Setup;
using Domain.Entities.Setup;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Vacancy : BaseEntity 
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int SalaryMin { get; set; }
        public int SalaryMax { get; set; }
        public TypeOfEmployment TypeOfEmployment { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime DeadlineDate { get; set; }

        public virtual ICollection<Level> Level { get; set; }
        public virtual ICollection<Location> Locations { get; set; }
        public virtual ICollection<Skill> RequiredSkills { get; set; }
        public virtual ICollection<VacancyStageInfo> CandidatesProgress { get; set; }
        public virtual ICollection<File> Files { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        //public virtual ICollection<Event> Events { get; set; }


        public virtual Vacancy ParentVacancy { get; set; }
        public virtual Industry Industry { get; set; }

        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }

        public int ResponsibleId { get; set; }
        public virtual User Responsible { get; set; }

        public int LanguageSkillId { get; set; }
        public virtual LanguageSkill LanguageSkill { get; set; }


    }
}
