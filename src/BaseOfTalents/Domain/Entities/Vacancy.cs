using BaseOfTalents.Domain.Entities.Enum;
using BaseOfTalents.Domain.Entities.Enum.Setup;
using System;
using System.Collections.Generic;

namespace BaseOfTalents.Domain.Entities
{
    public class Vacancy : BaseEntity
    {
        public Vacancy()
        {
            Levels = new List<Level>();
            Locations = new List<Location>();
            RequiredSkills = new List<Skill>();
            CandidatesProgress = new List<VacancyStageInfo>();
            Files = new List<File>();
            Comments = new List<Comment>();
            Tags = new List<Tag>();
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public int SalaryMin { get; set; }
        public int SalaryMax { get; set; }
        public TypeOfEmployment? TypeOfEmployment { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? DeadlineDate { get; set; }

        public virtual ICollection<Level> Levels { get; set; }
        public virtual ICollection<Location> Locations { get; set; }
        public virtual ICollection<Skill> RequiredSkills { get; set; }
        public virtual ICollection<VacancyStageInfo> CandidatesProgress { get; set; }
        public virtual ICollection<File> Files { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }

        public int? ParentVacancyId { get; set; }
        public virtual Vacancy ParentVacancy { get; set; }

        public int? IndustryId { get; set; }
        public virtual Industry Industry { get; set; }

        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }

        public int ResponsibleId { get; set; }
        public virtual User Responsible { get; set; }

        public virtual LanguageSkill LanguageSkill { get; set; }
    }
}