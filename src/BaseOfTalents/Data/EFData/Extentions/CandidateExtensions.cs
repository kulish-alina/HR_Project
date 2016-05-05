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

            domain.IndustryId = dto.IndustryId;

            foreach (var dtoSocial in dto.SocialNetworks.ToList())
            {
                var socialdb = domain.SocialNetworks.ToList().FirstOrDefault(x => x.Id == dtoSocial.Id);
                if (socialdb == null) //Is Domain Entity Doesnt Contains That Social
                {
                    domain.SocialNetworks.Add(new CandidateSocial()  //add to domain entity
                    {
                        Path = dtoSocial.Path,
                        SocialNetworkId = dtoSocial.SocialNetworkId,
                    });
                }
                else                                            //if CONTAINS replace with freAsh information
                {
                    socialdb.Path = dtoSocial.Path;
                    socialdb.SocialNetworkId = dtoSocial.SocialNetworkId;
                }
            }
            foreach (var domainSocial in domain.SocialNetworks.ToList())
            {
                if (!dto.SocialNetworks.Any(x => x.Id == domainSocial.Id)) //is domain entity contains social that DTO doesnt contains 
                {
                    domain.SocialNetworks.Remove(domainSocial); //delete from domain entity
                }
            }


            foreach (var languageSkill in dto.LanguageSkills.ToList())
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
                }
            }
            foreach (var domainLangSkill in domain.LanguageSkills.ToList())
            {
                if(!dto.LanguageSkills.Any(x=> x.Id == domainLangSkill.Id))
                {
                    domain.LanguageSkills.Remove(domainLangSkill);
                }
            }



            foreach (var source in dto.Sources.ToList())
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
            foreach (var domainSource in domain.Sources.ToList())
            {
                if(!dto.Sources.Any( x=> x.Id == domainSource.Id))
                {
                    domain.Sources.Remove(domainSource);
                }    
            }
            
            foreach (var dtoVp in dto.VacanciesProgress.ToList())
            {
                var domainVp = domain.VacanciesProgress.FirstOrDefault(x => x.VacancyStage.VacancyId == dtoVp.VacancyStage.VacancyId);
                if (domainVp == null)
                {
                    domain.VacanciesProgress.Add(new VacancyStageInfo()
                    {
                        CandidateId = dtoVp.CandidateId == default(int) ? domain.Id : dtoVp.CandidateId,
                        VacancyStage = new VacancyStage()
                        {
                            IsCommentRequired = dtoVp.VacancyStage.IsCommentRequired,
                            Order = dtoVp.VacancyStage.Order,
                            StageId = dtoVp.VacancyStage.StageId,
                            VacancyId = dtoVp.VacancyStage.VacancyId,
                        },
                        Comment = dtoVp.Comment != null ? new Comment() { Message = dtoVp.Comment.Message } : null
                    });
                }
                else
                {
                    domainVp.CandidateId = dtoVp.CandidateId == default(int) ? domain.Id : dtoVp.CandidateId;
                    domainVp.VacancyStage.IsCommentRequired = dtoVp.VacancyStage.IsCommentRequired;
                    domainVp.VacancyStage.Order = dtoVp.VacancyStage.Order;
                    domainVp.VacancyStage.StageId = dtoVp.VacancyStage.StageId;
                    domainVp.Comment = dtoVp.Comment != null ? new Comment() { Message = dtoVp.Comment.Message } : domainVp.Comment;
                }
            }
            foreach (var domainVp in domain.VacanciesProgress.ToList())
            {
                if (!dto.VacanciesProgress.Any(x => x.VacancyStage.VacancyId == domainVp.VacancyStage.VacancyId))
                {
                    domain.VacanciesProgress.Remove(domainVp);
                }
            }

            foreach (var dtoTagId in dto.TagIds.ToList())
            {
                if (!domain.Tags.Any(x => x.Id == dtoTagId))
                {
                    domain.Tags.Add(tagRepo.Get(dtoTagId));
                }
            }

            foreach (var domainTag in domain.Tags.ToList())
            {
                if (!dto.TagIds.Any(x => x== domainTag.Id))
                {
                    domain.Tags.Remove(domainTag);
                }
            }
            
            foreach (var dtoPhone in dto.PhoneNumbers.ToList())
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
            foreach (var domainPhone in domain.PhoneNumbers.ToList())
            {
                if (!dto.PhoneNumbers.Any(x => x.Id == domainPhone.Id))
                {
                    domain.PhoneNumbers.Remove(domainPhone);
                }
            }

            foreach (var skillId in dto.SkillIds.ToList())
            {
                if(!domain.Skills.Any(x=>x.Id==skillId))
                {
                    domain.Skills.Add(skillRepo.Get(skillId));
                }
            }

            foreach (var domainSkill in domain.Skills.ToList())
            {
                if (!dto.SkillIds.Any(x => x == domainSkill.Id))
                {
                    domain.Skills.Remove(domainSkill);
                }
            }

            if (dto.Photo != null)
            {
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
        }
    }
}
