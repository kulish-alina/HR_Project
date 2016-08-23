using Domain.Entities.Enum;
using Domain.Entities.Enum.Setup;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Candidate : BaseEntity
    {
        public Candidate()
        {
            Tags = new List<Tag>();
            PhoneNumbers = new List<PhoneNumber>();
            Skills = new List<Skill>();
            SocialNetworks = new List<CandidateSocial>();
            LanguageSkills = new List<LanguageSkill>();
            Files = new List<File>();
            VacanciesProgress = new List<VacancyStageInfo>();
            Comments = new List<Comment>();
            Sources = new List<CandidateSource>();
            RelocationPlaces = new List<RelocationPlace>();
            Events = new List<Event>();
            ClosedVacancies = new List<Vacancy>();
        }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public bool? IsMale { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Email { get; set; }
        public string Skype { get; set; }
        public string PositionDesired { get; set; }
        public TypeOfEmployment? TypeOfEmployment { get; set; }
        public DateTime? StartExperience { get; set; }
        public string Practice { get; set; }
        public string Description { get; set; }

        public int? SalaryDesired { get; set; }
        public int? CurrencyId { get; set; }
        public virtual Currency Currency { get; set; }

        public bool? RelocationAgreement { get; set; }
        public virtual ICollection<RelocationPlace> RelocationPlaces { get; set; }

        public virtual ICollection<CandidateSource> Sources { get; set; }
        public virtual int? MainSourceId { get; set; }
        public virtual Source MainSource { get; set; }

        public int? CityId { get; set; }
        public virtual City City { get; set; }

        public int? LevelId { get; set; }
        public virtual Level Level { get; set; }

        public int IndustryId { get; set; }
        public virtual Industry Industry { get; set; }

        public string Education { get; set; }

        public virtual ICollection<Vacancy> ClosedVacancies { get; set; }
        public virtual ICollection<CandidateSocial> SocialNetworks { get; set; }
        public virtual ICollection<LanguageSkill> LanguageSkills { get; set; }
        public virtual ICollection<File> Files { get; set; }
        public virtual ICollection<VacancyStageInfo> VacanciesProgress { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<PhoneNumber> PhoneNumbers { get; set; }
        public virtual ICollection<Skill> Skills { get; set; }
        public virtual ICollection<Event> Events { get; set; }

        public virtual File Photo { get; set; }
    }
}