using Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.DTO
{
    public class CandidateDTO : BaseEntityDTO
    {
        public CandidateDTO()
        {
            SkillIds = new List<int>();
            TagIds = new List<int>();
            PhoneNumbers = new List<PhoneNumberDTO>();
            SocialNetworks = new List<CandidateSocialDTO>();
            LanguageSkills = new List<LanguageSkillDTO>();
            VacanciesProgress = new List<VacancyStageInfoDTO>();
            Sources = new List<CandidateSourceDTO>();
            Events = new List<EventDTO>();
            Files = new List<FileDTO>();
            Comments = new List<CommentDTO>();
            RelocationPlaces = new List<RelocationPlaceDTO>();
            ClosedVacanciesIds = new List<int>();
        }

        [Required]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Education { get; set; }
        public bool? IsMale { get; set; }
        public DateTime? BirthDate { get; set; }
        public FileDTO Photo { get; set; }
        public string Email { get; set; }
        public string Skype { get; set; }
        public string PositionDesired { get; set; }
        public int? SalaryDesired { get; set; }
        public TypeOfEmployment? TypeOfEmployment { get; set; }
        public string Practice { get; set; }
        public DateTime? StartExperience { get; set; }
        public string Description { get; set; }

        public bool? RelocationAgreement { get; set; }
        public IEnumerable<RelocationPlaceDTO> RelocationPlaces { get; set; }

        [Required]
        public int IndustryId { get; set; }
        public int? LevelId { get; set; }
        public int? CurrencyId { get; set; }
        public int? CityId { get; set; }

        public IEnumerable<int> ClosedVacanciesIds { get; set; }

        public IEnumerable<CandidateSourceDTO> Sources { get; set; }
        public int? MainSourceId { get; set; }

        public IEnumerable<int> SkillIds { get; set; }
        public IEnumerable<int> TagIds { get; set; }
        public IEnumerable<PhoneNumberDTO> PhoneNumbers { get; set; }
        public IEnumerable<CandidateSocialDTO> SocialNetworks { get; set; }
        public IEnumerable<LanguageSkillDTO> LanguageSkills { get; set; }
        public IEnumerable<VacancyStageInfoDTO> VacanciesProgress { get; set; }
        public IEnumerable<FileDTO> Files { get; set; }
        public IEnumerable<CommentDTO> Comments { get; set; }
        public IEnumerable<EventDTO> Events { get; set; }
    }
}