using Domain.Entities;
using WebApi.DTO.DTOModels;

namespace WebApi.DTO
{
    public static class DTOService
    {
        public static CandidateDTO CandidateToDTO(Candidate candidate)
        {
            return new CandidateDTO()
            {
                Id = candidate.Id,
                EditTime = candidate.EditTime,
                BirthDate = candidate.BirthDate,
                Comments = candidate.Comments,
                Description = candidate.Description,
                Education = candidate.Education,
                Email = candidate.Email,
                Experience = candidate.Experience,
                Files = candidate.Files,
                FirstName = candidate.FirstName,
                IsMale = candidate.IsMale,
                LanguageSkills = candidate.LanguageSkills,
                LastName = candidate.LastName,
                City = candidate.City,
                MiddleName = candidate.MiddleName,
                PhoneNumbers = candidate.PhoneNumbers,
                Photo = candidate.Photo,
                PositionDesired = candidate.PositionDesired,
                Practice = candidate.Practice,
                RelocationAgreement = candidate.RelocationAgreement,
                SalaryDesired = candidate.SalaryDesired,
                Skills = candidate.Skills,
                Skype = candidate.Skype,
                SocialNetworks = candidate.SocialNetworks,
                Sources = candidate.Sources,
                TypeOfEmployment = candidate.TypeOfEmployment,
                VacanciesProgress = candidate.VacanciesProgress,
                State = candidate.State
            }; 
        }
        public static Candidate DTOToCandidate(CandidateDTO dto)
        {
            return new Candidate()
            {
                Id = dto.Id,
                EditTime = dto.EditTime,
                BirthDate = dto.BirthDate,
                Comments = dto.Comments,
                Description = dto.Description,
                Education = dto.Education,
                Email = dto.Email,
                Experience = dto.Experience,
                Files = dto.Files,
                FirstName = dto.FirstName,
                IsMale = dto.IsMale,
                LanguageSkills = dto.LanguageSkills,
                LastName = dto.LastName,
                City = dto.City,
                MiddleName = dto.MiddleName,
                PhoneNumbers = dto.PhoneNumbers,
                Photo = dto.Photo,
                PositionDesired = dto.PositionDesired,
                Practice = dto.Practice,
                RelocationAgreement = dto.RelocationAgreement,
                SalaryDesired = dto.SalaryDesired,
                Skills = dto.Skills,
                Skype = dto.Skype,
                SocialNetworks = dto.SocialNetworks,
                Sources = dto.Sources,
                TypeOfEmployment = dto.TypeOfEmployment,
                VacanciesProgress = dto.VacanciesProgress,
                State = dto.State
            };
        }
        public static VacancyDTO VacancyToDTO(Vacancy vacancy)
        {
            return new VacancyDTO() {
                CandidatesProgress = vacancy.CandidatesProgress,
                State = vacancy.State,
                Comments = vacancy.Comments,
                DeadlineDate = vacancy.DeadlineDate,
                LanguageSkill = vacancy.LanguageSkill,
                Description = vacancy.Description,
                EditTime = vacancy.EditTime,
                EndDate = vacancy.EndDate,
                Files = vacancy.Files,
                Id = vacancy.Id,
                ParentVacancy = vacancy.ParentVacancy,
                Level = vacancy.Level,
                City = vacancy.City,
                Team = vacancy.Team,
                Title = vacancy.Title,
                RequiredSkills = vacancy.RequiredSkills,
                Responsible = vacancy.Responsible,
                SalaryMax = vacancy.SalaryMax,
                SalaryMin = vacancy.SalaryMin,
                StartDate = vacancy.StartDate,
                TypeOfEmployment = vacancy.TypeOfEmployment
            };
        }
        public static Vacancy DTOToVacancy(VacancyDTO vacancy)
        {
            return new Vacancy()
            {
                CandidatesProgress = vacancy.CandidatesProgress,
                Title = vacancy.Title,
                Team = vacancy.Team,
                ParentVacancy = vacancy.ParentVacancy,
                LanguageSkill = vacancy.LanguageSkill,
                State = vacancy.State,
                Comments = vacancy.Comments,
                DeadlineDate = vacancy.DeadlineDate,
                Description = vacancy.Description,
                EditTime = vacancy.EditTime,
                EndDate = vacancy.EndDate,
                Files = vacancy.Files,
                Id = vacancy.Id,
                Level = vacancy.Level,
                City = vacancy.City,
                RequiredSkills = vacancy.RequiredSkills,
                Responsible = vacancy.Responsible,
                SalaryMax = vacancy.SalaryMax,
                SalaryMin = vacancy.SalaryMin,
                StartDate = vacancy.StartDate,
                TypeOfEmployment = vacancy.TypeOfEmployment
            };
        }
    }
}