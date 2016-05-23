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
        public static void Update(this Candidate destination, CandidateDTO source, 
            IRepository<Skill> skillRepository, 
            IRepository<Tag> tagRepository, 
            IRepository<CandidateSocial> candidateSocialRepository, 
            IRepository<LanguageSkill> languageSkillRepository,
            IRepository<CandidateSource> candidateSourceRepository,
            IRepository<VacancyStageInfo> vacancyStageInfoRepository,
            IRepository<PhoneNumber> phoneNumberRepository,
            IRepository<Photo> photoRepository, 
            IRepository<Vacancy> vacancyRepository,
            IRepository<File> fileRepository)
        {
            destination.State               = source.State;
            destination.FirstName           = source.FirstName;
            destination.MiddleName          = source.MiddleName;
            destination.LastName            = source.LastName;
            destination.IsMale              = source.IsMale;
            destination.BirthDate           = source.BirthDate;
            destination.Email               = source.Email;
            destination.Skype               = source.Skype;
            destination.PositionDesired     = source.PositionDesired;
            destination.SalaryDesired       = source.SalaryDesired;
            destination.TypeOfEmployment    = source.TypeOfEmployment;
            destination.StartExperience     = source.StartExperience;
            destination.Practice            = source.Practice;
            destination.Description         = source.Description;
            destination.LocationId          = source.LocationId;
            destination.RelocationAgreement = source.RelocationAgreement;
            destination.Education           = source.Education;
            destination.IndustryId          = source.IndustryId;

            PerformSocialSaving(destination, source, candidateSocialRepository);
            PerformLanguageSkillsSaving(destination, source, languageSkillRepository);
            PerformSourcesSaving(destination, source, candidateSourceRepository);
            PerformVacanciesProgressSaving(destination, source, vacancyStageInfoRepository, vacancyRepository);
            PerformTagsSaving(destination, source, tagRepository);
            PerformPhoneNumbersSaving(destination, source, phoneNumberRepository);
            PerformSkillsSaving(destination, source, skillRepository);
            PerformPhotoSaving(destination, source, photoRepository);
            PerformFilesSaving(destination, source, fileRepository);
        }

        private static void PerformFilesSaving(Candidate destination, CandidateDTO source, IRepository<File> fileRepository)
        {
            source.Files.ToList().ForEach(file =>
            {
                var fileInVacancy = destination.Files.FirstOrDefault(x => x.Id == file.Id);
                var dbFile = fileRepository.Get(file.Id);
                if (dbFile == null)
                {
                    throw new Exception("Unknown file");
                }
                if (file.ShouldBeRemoved())
                {
                    fileRepository.Remove(file.Id);
                }
                else
                {
                    dbFile.Update(file);
                    if (fileInVacancy == null)
                    {
                        destination.Files.Add(dbFile);
                    }
                }
            });
        }

        private static void PerformPhotoSaving(Candidate destination, CandidateDTO source, IRepository<Photo> photoRepository)
        {
            if (source.Photo != null)
            {
                if (source.Photo.IsNew())
                {
                    var photoBd = photoRepository.Get(source.Photo.Id);
                    photoBd.Update(source.Photo);
                    destination.Photo = photoBd;
                }
                else if (source.Photo.ShouldBeRemoved())
                {
                    photoRepository.Remove(destination.Photo.Id);
                }
                else
                {
                    destination.Photo.Update(source.Photo);
                }
            }
        }

        private static void PerformSkillsSaving(Candidate destination, CandidateDTO source, IRepository<Skill> skillRepository)
        {
            destination.Skills.Clear();
            source.SkillIds.ToList().ForEach(skillId =>
            {
                destination.Skills.Add(skillRepository.Get(skillId));
            });
        }

        private static void PerformTagsSaving(Candidate destination, CandidateDTO source, IRepository<Tag> tagRepository)
        {
            destination.Tags.Clear();
            source.TagIds.ToList().ForEach(tagId =>
            {
                destination.Tags.Add(tagRepository.Get(tagId));
            });
        }

        private static void PerformPhoneNumbersSaving(Candidate destination, CandidateDTO source, IRepository<PhoneNumber> phoneNumberRepository)
        {
            RefreshExistingPhoneNumbers(destination, source, phoneNumberRepository);
            CreateNewPhoneNumbers(destination, source);
        }
        private static void CreateNewPhoneNumbers(Candidate destination, CandidateDTO source)
        {
            source.PhoneNumbers.Where(x => x.IsNew()).ToList().ForEach(newPhoneNumber =>
            {
                var toDomain = new PhoneNumber();
                toDomain.Update(newPhoneNumber);
                destination.PhoneNumbers.Add(toDomain);
            });
        }
        private static void RefreshExistingPhoneNumbers(Candidate destination, CandidateDTO source, IRepository<PhoneNumber> phoneNumberRepository)
        {
            source.PhoneNumbers.Where(x => !x.IsNew()).ToList().ForEach(updatedPhoneNumber =>
            {
                var domainPhoneNumber = destination.PhoneNumbers.FirstOrDefault(x => x.Id == updatedPhoneNumber.Id);
                if (domainPhoneNumber == null)
                {
                    throw new ArgumentNullException("Request contains unknown entity");
                }
                if (updatedPhoneNumber.ShouldBeRemoved())
                {
                    phoneNumberRepository.Remove(updatedPhoneNumber.Id);
                }
                else
                {
                    domainPhoneNumber.Update(updatedPhoneNumber);
                }
            });
        }

        private static void PerformVacanciesProgressSaving(Candidate destination, CandidateDTO source, IRepository<VacancyStageInfo> vacancyStageInfoRepository, IRepository<Vacancy> vacancyRepository)
        {
            RefreshExistingVacanciesProgress(destination, source, vacancyStageInfoRepository);
            CreateNewVacanciesProgress(destination, source);
        }
        private static void CreateNewVacanciesProgress(Candidate destination, CandidateDTO source)
        {
            source.VacanciesProgress.Where(x => x.IsNew()).ToList().ForEach(newVacancyStageInfo => 
            {
                var toDomain = new VacancyStageInfo();
                toDomain.Update(newVacancyStageInfo);
                destination.VacanciesProgress.Add(toDomain);
            });
        }
        private static void RefreshExistingVacanciesProgress(Candidate destination, CandidateDTO source, IRepository<VacancyStageInfo> vacancyStageInfoRepository)
        {
            source.VacanciesProgress.Where(x => !x.IsNew()).ToList().ForEach(updatedVacanciesStageInfo =>
            {
                var domainVacancyStageInfo = destination.VacanciesProgress.FirstOrDefault(x => x.Id == updatedVacanciesStageInfo.Id);
                if (domainVacancyStageInfo == null)
                {
                    throw new ArgumentNullException("Request contains unknown entity");
                }
                if (updatedVacanciesStageInfo.VacancyStage.IsCommentRequired)
                {
                    if (updatedVacanciesStageInfo.Comment == null)
                    {
                        throw new ArgumentNullException("Vacancy stage info should have comment");
                    }
                }
                if (updatedVacanciesStageInfo.ShouldBeRemoved())
                {
                    vacancyStageInfoRepository.Remove(updatedVacanciesStageInfo.Id);
                }
                else
                {
                    domainVacancyStageInfo.Update(updatedVacanciesStageInfo);
                }
            });
        }

        private static void PerformSourcesSaving(Candidate destination, CandidateDTO source, IRepository<CandidateSource> candidateSourceRepository)
        {
            RefreshExistingSources(destination, source, candidateSourceRepository);
            CreateNewSources(destination, source);
        }
        private static void CreateNewSources(Candidate destination, CandidateDTO source)
        {
            source.Sources.Where(x => x.IsNew()).ToList().ForEach(newSource=> 
            {
                var toDomain = new CandidateSource();
                toDomain.Update(newSource);
                destination.Sources.Add(toDomain);
            });
        }
        private static void RefreshExistingSources(Candidate destination, CandidateDTO source, IRepository<CandidateSource> candidateSourceRepository)
        {
            source.Sources.Where(x => !x.IsNew()).ToList().ForEach(updatedSource =>
            {
                var domainSource = destination.Sources.FirstOrDefault(x => x.Id == updatedSource.Id);
                if (domainSource == null)
                {
                    throw new ArgumentNullException("Request contains unknown entity");
                }
                if (updatedSource.ShouldBeRemoved())
                {
                    candidateSourceRepository.Remove(updatedSource.Id);
                }
                else
                {
                    domainSource.Update(updatedSource);
                }
            });
        }

        private static void PerformLanguageSkillsSaving(Candidate destination, CandidateDTO source, IRepository<LanguageSkill> languageSkillRepository)
        {
            CreateNewLanguageSkills(destination, source);
            RefreshExistingLanguageSkills(destination, source, languageSkillRepository);
        }
        private static void CreateNewLanguageSkills(Candidate destination, CandidateDTO source)
        {
            source.LanguageSkills.Where(x => x.IsNew()).ToList().ForEach(newLanguageSkill =>
            {
                var toDomain = new LanguageSkill();
                toDomain.Update(newLanguageSkill);
                destination.LanguageSkills.Add(toDomain);
            });
        }
        private static void RefreshExistingLanguageSkills(Candidate destination, CandidateDTO source, IRepository<LanguageSkill> languageSkillRepository)
        {
            source.LanguageSkills.Where(x => !x.IsNew()).ToList().ForEach(updatedLanguageSkills =>
            {
                var domainLS = destination.LanguageSkills.ToList().FirstOrDefault(x => x.Id == updatedLanguageSkills.Id);
                if (domainLS == null)
                {
                    throw new ArgumentNullException("Request contains unknown entity");
                }
                if (updatedLanguageSkills.ShouldBeRemoved())
                {
                    languageSkillRepository.Remove(updatedLanguageSkills.Id);
                }
                else
                {
                    domainLS.Update(updatedLanguageSkills);
                }
            });
        }

        private static void PerformSocialSaving(Candidate destination, CandidateDTO source, IRepository<CandidateSocial> candidateSocialRepository)
        {
            RefreshExistingSocials(destination, source, candidateSocialRepository);
            CreateNewSocials(destination, source);
        }
        private static void CreateNewSocials(Candidate destination, CandidateDTO source)
        {
            source.SocialNetworks.Where(x => x.IsNew()).ToList().ForEach(newSocial =>
            {
                var toDomain = new CandidateSocial();
                toDomain.Update(newSocial);
                destination.SocialNetworks.Add(toDomain);
            });
        }
        private static void RefreshExistingSocials(Candidate destination, CandidateDTO source, IRepository<CandidateSocial> candidateSocialRepository)
        {
            source.SocialNetworks.Where(x => !x.IsNew()).ToList().ForEach(updatedSocial =>
            {
                var domainSocial = destination.SocialNetworks.ToList().FirstOrDefault(x => x.Id == updatedSocial.Id);
                if (domainSocial == null)
                {
                    throw new ArgumentNullException("Request contains unknown entity");
                }
                if (updatedSocial.ShouldBeRemoved())
                {
                    candidateSocialRepository.Remove(updatedSocial.Id);
                }
                else
                {
                    domainSocial.Update(updatedSocial);
                }
            });
        }
    }
}
