using Domain.Entities;
using Domain.Entities.Enum;
using Domain.Entities.Setup;
using Domain.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.DTO.DTOModels
{
    public class CandidateDTO
    {
        public CandidateDTO()
        {
            PhoneNumbers = new List<string>();
            Skills = new List<Skill>();
            SocialNetworks = new List<CandidateSocial>();
            LanguageSkills = new List<LanguageSkill>();
            Files = new List<File>();
            VacanciesProgress = new List<VacancyStageInfo>();
            Comments = new List<Comment>();
            Sources = new List<CandidateSource>();
        }
        public int Id { get; set; }
        public DateTime EditTime { get; set; }
        public EntityState State { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public bool IsMale { get; set; }
        public DateTime BirthDate { get; set; }
        public virtual Photo Photo { get; set; }
        public List<string> PhoneNumbers { get; set; }
        public string Email { get; set; }
        public string Skype { get; set; }
        public string PositionDesired { get; set; }
        public int SalaryDesired { get; set; }
        public virtual List<Skill> Skills { get; set; }
        public TypeOfEmployment TypeOfEmployment { get; set; }
        public string Practice { get; set; }
        public Experience Experience { get; set; }
        public string Description { get; set; }
        public virtual City City { get; set; }
        public bool RelocationAgreement { get; set; }
        public virtual List<CandidateSocial> SocialNetworks { get; set; }
        public string Education { get; set; }
        public virtual List<LanguageSkill> LanguageSkills { get; set; }
        public virtual List<File> Files { get; set; }
        [JsonIgnore]
        public virtual List<VacancyStageInfo> VacanciesProgress { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<CandidateSource> Sources { get; set; }

    }
}