using BotLibrary.Entities;
using BotWebApi.DTO.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BotWebApi.DTO
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
                Gender = candidate.Gender,
                Languages = candidate.Languages,
                LastName = candidate.LastName,
                Location = candidate.Location,
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
                VacanciesProgress = candidate.VacanciesProgress
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
                Gender = dto.Gender,
                Languages = dto.Languages,
                LastName = dto.LastName,
                Location = dto.Location,
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
                VacanciesProgress = dto.VacanciesProgress
            };
        }
        public static VacancyDTO VacancyToDTO(Vacancy vacancy)
        {
            return new VacancyDTO() {
                CandidatesProgress = vacancy.CandidatesProgress,
                ChildredVacanciesCount = vacancy.ChildredVacanciesCount,
                Comments = vacancy.Comments,
                DeadlineDate = vacancy.DeadlineDate,
                Department = vacancy.Department,
                Description = vacancy.Description,
                EditTime = vacancy.EditTime,
                EndDate = vacancy.EndDate,
                Files = vacancy.Files,
                Id = vacancy.Id,
                IsDeadlineAddedToCalendar = vacancy.IsDeadlineAddedToCalendar,
                Level = vacancy.Level,
                Location = vacancy.Location,
                MasterVacancy = vacancy.MasterVacancy,
                Name = vacancy.Name,
                RequiredLanguages = vacancy.RequiredLanguages,
                RequiredSkills = vacancy.RequiredSkills,
                Responsible = vacancy.Responsible,
                SalaryMax = vacancy.SalaryMax,
                SalaryMin = vacancy.SalaryMin,
                StartDate = vacancy.StartDate,
                Status = vacancy.Status,
                TypeOfEmployment = vacancy.TypeOfEmployment
            };
        }
        public static Vacancy DTOToVacancy(VacancyDTO vacancy)
        {
            return new Vacancy()
            {
                CandidatesProgress = vacancy.CandidatesProgress,
                ChildredVacanciesCount = vacancy.ChildredVacanciesCount,
                Comments = vacancy.Comments,
                DeadlineDate = vacancy.DeadlineDate,
                Department = vacancy.Department,
                Description = vacancy.Description,
                EditTime = vacancy.EditTime,
                EndDate = vacancy.EndDate,
                Files = vacancy.Files,
                Id = vacancy.Id,
                IsDeadlineAddedToCalendar = vacancy.IsDeadlineAddedToCalendar,
                Level = vacancy.Level,
                Location = vacancy.Location,
                MasterVacancy = vacancy.MasterVacancy,
                Name = vacancy.Name,
                RequiredLanguages = vacancy.RequiredLanguages,
                RequiredSkills = vacancy.RequiredSkills,
                Responsible = vacancy.Responsible,
                SalaryMax = vacancy.SalaryMax,
                SalaryMin = vacancy.SalaryMin,
                StartDate = vacancy.StartDate,
                Status = vacancy.Status,
                TypeOfEmployment = vacancy.TypeOfEmployment
            };
        }
    }
}