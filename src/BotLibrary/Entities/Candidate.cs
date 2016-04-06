using BotLibrary.Entities.Enum;
using BotLibrary.Entities.Setup;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BotLibrary.Entities
{
    public class Candidate: BaseEntity
    {
        public Candidate()
        {
            PhoneNumbers = new List<string>();
            Skills = new List<Skill>();
            SocialNetworks = new List<SocialNetwork>();
            Languages = new List<Language>();
            Files = new List<File>();
            VacanciesProgress = new List<VacancyStageInfo>();
            Comments = new List<Comment>();
            Sources = new List<Source>();
        }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public bool Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public Photo Photo { get; set; }
        public List<string> PhoneNumbers { get; set; }
        public string Email { get; set; }
        public string Skype { get; set; }
        public string PositionDesired { get; set; }
        public int SalaryDesired { get; set; }
        public List<Skill> Skills { get; set; }
        public TypeOfEmployment TypeOfEmployment { get; set; }
        public string Practice { get; set; }
        public Experience Experience { get; set; }
        public string Description { get; set; }
        public Location Location { get; set; }
        public bool RelocationAgreement { get; set; }
        public List<SocialNetwork> SocialNetworks { get; set; }
        public string Education { get; set; }
        public List<Language> Languages { get; set; }
        public List<File> Files { get; set; }
        public List<VacancyStageInfo> VacanciesProgress { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Source> Sources { get; set; }
    }
}
