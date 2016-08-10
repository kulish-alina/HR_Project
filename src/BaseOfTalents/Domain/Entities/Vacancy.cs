using Domain.Entities.Enum;
using Domain.Entities.Enum.Setup;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Vacancy : BaseEntity
    {
        public Vacancy()
        {
            Levels = new List<Level>();
            Cities = new List<City>();
            RequiredSkills = new List<Skill>();
            CandidatesProgress = new List<VacancyStageInfo>();
            Files = new List<File>();
            Comments = new List<Comment>();
            Tags = new List<Tag>();
            ChildVacancies = new List<Vacancy>();
            StageFlow = new List<ExtendedStage>();
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public TypeOfEmployment? TypeOfEmployment { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? DeadlineDate { get; set; }
        public bool DeadlineToCalendar { get; set; }

        public virtual ICollection<Level> Levels { get; set; }
        public virtual ICollection<City> Cities { get; set; }
        public virtual ICollection<Skill> RequiredSkills { get; set; }
        public virtual ICollection<ExtendedStage> StageFlow { get; set; }
        public virtual ICollection<VacancyStageInfo> CandidatesProgress { get; set; }
        public virtual ICollection<File> Files { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }

        public int? ChildVacanciesNumber { get; set; }
        public virtual ICollection<Vacancy> ChildVacancies { get; set; }

        public int? ClosingCandidateId { get; set; }
        public Candidate ClosingCandidate { get; set; }

        public int? ParentVacancyId { get; set; }
        public virtual Vacancy ParentVacancy { get; set; }

        public int IndustryId { get; set; }
        public virtual Industry Industry { get; set; }

        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }

        public int ResponsibleId { get; set; }
        public virtual User Responsible { get; set; }

        public int? SalaryMin { get; set; }
        public int? SalaryMax { get; set; }
        public int? CurrencyId { get; set; }
        public virtual Currency Currency { get; set; }

        public virtual LanguageSkill LanguageSkill { get; set; }
    }
}