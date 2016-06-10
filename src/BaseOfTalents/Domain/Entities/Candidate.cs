using BaseOfTalents.Domain.Entities.Enum;
using BaseOfTalents.Domain.Entities.Enum.Setup;
using Domain.Entities.Enum.Setup;
using System;
using System.Collections.Generic;

namespace BaseOfTalents.Domain.Entities
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
        }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public bool IsMale { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string Skype { get; set; }
        public string PositionDesired { get; set; }
        public TypeOfEmployment TypeOfEmployment { get; set; }
        public DateTime StartExperience { get; set; }
        public string Practice { get; set; }
        public string Description { get; set; }

        public int SalaryDesired { get; set; }
        public int? CurrencyId { get; set; }
        public virtual Currency Currency { get; set; }

        public int LocationId { get; set; }
        public virtual Location Location { get; set; }

        public int? LevelId { get; set; }
        public virtual Level Level { get; set; }

        public int? IndustryId { get; set; }
        public virtual Industry Industry { get; set; }

        public bool RelocationAgreement { get; set; }
        public int? RelocationPlaceId { get; set; }
        public Location RelocationPlace { get; set; }

        public string Education { get; set; }

        public virtual ICollection<CandidateSocial> SocialNetworks { get; set; }
        public virtual ICollection<LanguageSkill> LanguageSkills { get; set; }
        public virtual ICollection<File> Files { get; set; }
        public virtual ICollection<VacancyStageInfo> VacanciesProgress { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<CandidateSource> Sources { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<PhoneNumber> PhoneNumbers { get; set; }
        public virtual ICollection<Skill> Skills { get; set; }

        public virtual Photo Photo { get; set; }
    }
}