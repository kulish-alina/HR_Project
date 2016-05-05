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

            foreach (var dtoSocial in dto.SocialNetworks)
            {
                if (domain.SocialNetworks.Any(x => x.Id == dtoSocial.Id))
                {

                }
            }

            foreach (var dtoSocial in dto.SocialNetworks)
            {

                if (!domain.SocialNetworks.Any(x => x.Id == dtoSocial.Id)) //Is Domain Entity Doesnt Contains That Social
                {
                    domain.SocialNetworks.Add(new CandidateSocial()  //add to domain entity
                    {
                        Path = dtoSocial.Path,
                        SocialNetworkId = dtoSocial.SocialNetworkId,
                    });
                }
            }

            foreach (var domainSocial in domain.SocialNetworks)
            {
                if (!dto.SocialNetworks.Any(x => x.Id == domainSocial.Id)) //is domain entity contains social that DTO doesnt contains 
                {
                    domain.SocialNetworks.Remove(domainSocial); //delete from domain entity
                }
            }


            foreach (var social in dto.SocialNetworks)
            {
                var domainSocial = domain.SocialNetworks.FirstOrDefault(x => x.Id == social.Id);
                if (domainSocial == null)
                {
                    domain.SocialNetworks.Add(new CandidateSocial()
                    {
                        Path = social.Path,
                        SocialNetworkId = social.SocialNetworkId,
                    });
                }
                else
                {
                    domainSocial.Path = social.Path;
                    domainSocial.SocialNetworkId = social.SocialNetworkId;
                    domainSocial.State = social.State;
                }
            }

            foreach (var languageSkill in dto.LanguageSkills)
            {
                var domainLangSkill = domain.LanguageSkills.FirstOrDefault(x => x.Id == languageSkill.Id);
                if (domainLangSkill == null)
                {
                    domain.LanguageSkills.Add(new LanguageSkill()
                    {
                       LanguageId = languageSkill.LanguageId,
                       LanguageLevel = languageSkill.LanguageLevel,
                    });
                }
                else
                {
                    domainLangSkill.LanguageId = languageSkill.LanguageId;
                    domainLangSkill.LanguageLevel = languageSkill.LanguageLevel;
                    domainLangSkill.State = languageSkill.State;
                }
            }

            foreach (var source in dto.Sources)
            {
                var domainSource = domain.Sources.FirstOrDefault(x => x.Id == source.Id);
                if (domainSource == null)
                {
                    domain.Sources.Add(new CandidateSource()
                    {
                        Path = source.Path,
                        Source = source.Source,
                    });
                }
                else
                {
                    domainSource.Path = source.Path;
                    domainSource.Source = source.Source;
                    domainSource.State = source.State;
                }
            }

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


            foreach (var tagId in dto.TagIds)
            {
                if(!domain.Tags.Any(x=>x.Id == tagId))
                {
                    domain.Tags.Add(tagRepo.Get(tagId));
                }
            }

            foreach (var dtoPhone in dto.PhoneNumbers)
            {
                var domainPhone = domain.PhoneNumbers.FirstOrDefault(x => x.Id == dtoPhone.Id);
                if (domainPhone == null)
                {
                    var number = new PhoneNumber()
                    {
                        Number = dtoPhone.Number
                    };
                    domain.PhoneNumbers.Add(number);
                }
                else
                {
                    domainPhone.Number = dtoPhone.Number;
                    domainPhone.State = dtoPhone.State;
                }
            }

            foreach (var skillId in dto.SkillIds)
            {
                if(!domain.Skills.Any(x=>x.Id==skillId))
                {
                    domain.Skills.Add(skillRepo.Get(skillId));
                }
            }
            domain.IndustryId = dto.IndustryId;

            if (dto.Photo.Id != 0)
            {
                var photoBd = domain.Photo;
                photoBd.Description = dto.Photo.Description;
                photoBd.ImagePath = dto.Photo.ImagePath;
                photoBd.State = dto.Photo.State;
            }
            else
            {
                domain.Photo = new Photo
                {
                    Id = dto.Photo.Id,
                    Description = dto.Photo.Description,
                    ImagePath = dto.Photo.ImagePath,
                    State = dto.Photo.State
                };
            }
        }

        private static bool IsDomainEntityContainsSocial()
        {
            throw new NotImplementedException();
        }
    }
}
