using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BotLibrary.Entities;

namespace BotWebApi.DTOs.CandidateDTO
{
    public static class CandidateExtentions
    {
        public static CandidateDTO ToDTO(this Candidate candidate)
        {
            return new CandidateDTO()
            {
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
                VacanciesProgress = candidate.VacanciesProgress.Select(x => new SimplyfiedVacancyStageInfo()
                {
                    VacancyId = x.Vacancy.Id,
                    StageInfos = x.StageInfos
                }).ToList()
            };
        }
    }
}