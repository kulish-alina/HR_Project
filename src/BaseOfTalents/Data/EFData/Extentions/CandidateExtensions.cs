using Domain.DTO.DTOModels;
using Domain.Entities;
using Domain.Entities.Enum.Setup;
using Domain.Entities.Setup;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EFData.Extentions
{
    public static class CandidateExtensions
    {
        public static void Update(this Candidate domain, CandidateDTO dto, IRepository<Skill> skillRepo, IRepository<Tag> tagRepo)
        {
            domain.State = dto.State;

            domain.FirstName = dto.FirstName;
            domain.MiddleName = dto.MiddleName;
            domain.LastName = dto.LastName;
            domain.IsMale = dto.IsMale;
            domain.BirthDate = dto.BirthDate;

            domain.Email = dto.Email;
            domain.Skype = dto.Skype;
            domain.PositionDesired = dto.PositionDesired;
            domain.SalaryDesired = dto.SalaryDesired;
            domain.TypeOfEmployment = dto.TypeOfEmployment;
            domain.StartExperience = dto.StartExperience;
            domain.Practice = dto.Practice;
            domain.Description = dto.Description;

            domain.LocationId = dto.LocationId;
            domain.RelocationAgreement = dto.RelocationAgreement;
            domain.Education = dto.Education;

            domain.SocialNetworks = dto.SocialNetworks.Select(x => new CandidateSocial()
            {
                Id = x.Id,
                Path = x.Path,
                State = x.State,
                SocialNetworkId = x.SocialNetworkId
            }).ToList();

            domain.LanguageSkills = dto.LanguageSkills.Select(x => new LanguageSkill()
            {
                Id = x.Id,
                State = x.State,
                LanguageId = x.LanguageId,
                LanguageLevel = x.LanguageLevel
            }).ToList();

            domain.VacanciesProgress = dto.VacanciesProgress.Select(x => new VacancyStageInfo()
            {
                Id = x.Id,
                CandidateId = x.CandidateId,
                State = x.State,
                VacancyStage = new VacancyStage()
                {
                    Id = x.Id,
                    State = x.State,
                    IsCommentRequired = x.VacancyStage.IsCommentRequired,
                    Order = x.VacancyStage.Order,
                    StageId = x.VacancyStage.StageId,
                    VacancyId = x.VacancyStage.VacancyId,
                }
            }).ToList();

            domain.Sources = dto.Sources.Select(x => new CandidateSource()
            {
                Id = x.Id,
                Path = x.Path,
                Source = x.Source,
                State = x.State
            }).ToList();

            domain.Tags = dto.TagIds.Select(x => tagRepo.Get(x)).ToList();
            domain.PhoneNumbers = dto.PhoneNumbers.Select(x => new PhoneNumber()
            {
                Id = x.Id,
                Number = x.Number,
                State = x.State
            }).ToList();
            domain.Skills = dto.SkillIds.Select(x => skillRepo.Get(x)).ToList();

            domain.IndustryId = dto.IndustryId;
            domain.Photo = new Photo()
            {
                Description = dto.Photo.Description,
                Id = dto.Photo.Id,
                ImagePath = dto.Photo.ImagePath,
                State = dto.Photo.State
            };
        }
    }
}
