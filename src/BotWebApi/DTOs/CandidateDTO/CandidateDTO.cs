using BotLibrary.Entities;
using BotLibrary.Entities.Enum;
using BotLibrary.Entities.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BotWebApi.DTOs.CandidateDTO
{
    public class CandidateDTO
    {
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
        public List<SimplyfiedVacancyStageInfo> VacanciesProgress { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Source> Sources { get; set; }
    }

}