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

            PerformSocialSaving(domain, dto);
            PerformLanguageSkillsSaving(domain, dto);
            PerformSourcesSaving(domain, dto);
            PerformVacanciesProgressSaving(domain, dto);
            
            
            

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

        private static void PerformVacanciesProgressSaving(Candidate destination, CandidateDTO source)
        {
            foreach (var dtoVp in source.VacanciesProgress.ToList())
            {
                var domainVp = destination.VacanciesProgress.FirstOrDefault(x => x.VacancyStage.VacancyId == dtoVp.VacancyStage.VacancyId);
                if (domainVp == null)
                {
                    destination.VacanciesProgress.Add(new VacancyStageInfo()
                    {
                        CandidateId = dtoVp.CandidateId == default(int) ? destination.Id : dtoVp.CandidateId,
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
                    domainVp.CandidateId = dtoVp.CandidateId == default(int) ? destination.Id : dtoVp.CandidateId;
                    domainVp.VacancyStage.IsCommentRequired = dtoVp.VacancyStage.IsCommentRequired;
                    domainVp.VacancyStage.Order = dtoVp.VacancyStage.Order;
                    domainVp.VacancyStage.StageId = dtoVp.VacancyStage.StageId;
                    domainVp.Comment = dtoVp.Comment != null ? new Comment() { Message = dtoVp.Comment.Message } : domainVp.Comment;
                }
            }
            foreach (var domainVp in destination.VacanciesProgress.ToList())
            {
                if (!source.VacanciesProgress.Any(x => x.VacancyStage.VacancyId == domainVp.VacancyStage.VacancyId))
                {
                    destination.VacanciesProgress.Remove(domainVp);
                }
            }


        }

        private static void PerformSourcesSaving(Candidate destination, CandidateDTO source)
        {
            RefreshExistingSources(destination, source);
            CreateNewSources(destination, source);
        }

        private static void CreateNewSources(Candidate destination, CandidateDTO source)
        {
            foreach (var newSource in source.Sources.Where(x=> x.Id == 0).ToList())
            {
                destination.Sources.Add(new CandidateSource()
                {
                    Path = newSource.Path,
                    Source = newSource.Source
                });
            }
        }

        private static void RefreshExistingSources(Candidate destination, CandidateDTO source)
        {
            foreach (var oldSource in source.Sources.Where(x => x.Id != 0))
            {
                var domainSource = destination.Sources.FirstOrDefault(x => x.Id == oldSource.Id);
                domainSource.Path = oldSource.Path;
                domainSource.Source = oldSource.Source;
                domainSource.State = oldSource.State;
            }
        }

        private static void PerformLanguageSkillsSaving(Candidate destination, CandidateDTO source)
        {
            RefreshExistingLS(destination, source);
            CreateNewLS(destination, source);
        }

        private static void CreateNewLS(Candidate destination, CandidateDTO source)
        {
            foreach (var newLS in source.LanguageSkills.Where(x => x.Id == 0).ToList())
            {
                destination.LanguageSkills.Add(new LanguageSkill()
                {
                    LanguageId = newLS.LanguageId,
                    LanguageLevel = newLS.LanguageLevel
                });
            }
        }

        private static void RefreshExistingLS(Candidate destination, CandidateDTO source)
        {
            foreach (var oldLS in source.LanguageSkills.Where(x => x.Id != 0).ToList())
            {
                var domainLS = destination.LanguageSkills.ToList().FirstOrDefault(x => x.Id == oldLS.Id);
                domainLS.LanguageLevel = oldLS.LanguageLevel;
                domainLS.LanguageId = oldLS.LanguageId;
                domainLS.State = oldLS.State;
            }
        }

        private static void PerformSocialSaving(Candidate destination, CandidateDTO source)
        {
            RefreshExistingSocials(destination, source);
            CreateNewSocials(destination, source);
        }

        private static void CreateNewSocials(Candidate destination, CandidateDTO source)
        {
            foreach (var newSocial in source.SocialNetworks.Where(x => x.Id == 0).ToList())
            {
                destination.SocialNetworks.Add(new CandidateSocial()
                {
                    Path = newSocial.Path,
                    SocialNetworkId = newSocial.SocialNetworkId,
                });
            }
        }

        private static void RefreshExistingSocials(Candidate destination, CandidateDTO source)
        {
            foreach (var oldSocial in source.SocialNetworks.Where(x => x.Id != 0).ToList())
            {
                var domainSocial = destination.SocialNetworks.ToList().FirstOrDefault(x => x.Id == oldSocial.Id);
                domainSocial.Path = oldSocial.Path;
                domainSocial.SocialNetworkId = oldSocial.SocialNetworkId;
                domainSocial.State = oldSocial.State;
            }
        }
    }
}
