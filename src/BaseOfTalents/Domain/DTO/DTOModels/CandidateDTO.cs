using Domain.Entities;
using Domain.Entities.Enum;
using Domain.Entities.Enum.Setup;
using Domain.Entities.Setup;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Domain.DTO.DTOModels
{
    public class CandidateDTO
    {
        public CandidateDTO()
        {
            PhoneNumbers = new List<PhoneNumber>();
            SkillsIds = new List<int>();
            SocialNetworks = new List<CandidateSocialDTO>();
            LanguageSkills = new List<LanguageSkillDTO>();
            Files = new List<File>();
            VacanciesProgress = new List<VacancyStageInfoDTO>();
            Comments = new List<Comment>();
            Sources = new List<CandidateSource>();
        }

        public int Id { get; set; }
        public DateTime EditTime { get; set; }
        public EntityState State { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string MiddleName { get; set; }
        [Required]
        public string LastName { get; set; }
        public bool IsMale { get; set; }
        public DateTime BirthDate { get; set; }
        public Industry Industry { get; set; }
        public Photo Photo { get; set; }
        public IEnumerable<PhoneNumber> PhoneNumbers { get; set; }
        [Required]
        public string Email { get; set; }
        public string Skype { get; set; }
        public string PositionDesired { get; set; }
        public int SalaryDesired { get; set; }
        public IEnumerable<int> SkillsIds { get; set; }
        public TypeOfEmployment TypeOfEmployment { get; set; }
        public string Practice { get; set; }
        public DateTime StartExperience { get; set; }
        public string Description { get; set; }
        public virtual int LocationId { get; set; }
        public bool RelocationAgreement { get; set; }
        public IEnumerable<CandidateSocialDTO> SocialNetworks { get; set; }
        public string Education { get; set; }
        public IEnumerable<LanguageSkillDTO> LanguageSkills { get; set; }
        public IEnumerable<File> Files { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
        public IEnumerable<VacancyStageInfoDTO> VacanciesProgress { get; set; }
        public IEnumerable<CandidateSource> Sources { get; set; }
        public IEnumerable<Tag> Tags { get; set; }

    }
}