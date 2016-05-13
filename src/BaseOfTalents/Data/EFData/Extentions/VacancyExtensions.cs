using Data.Infrastructure;
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
    public static class VacancyExtensions
    {
        public static void Update(this Vacancy destination, VacancyDTO source,
            IRepository<Level> levelRepository,
            IRepository<Location> locationRepository,
            IRepository<Skill> skillRepository,
            IRepository<Tag> tagRepository,
            IRepository<LanguageSkill> languageSkillRepository,
            IRepository<VacancyStageInfo> vacancyStageInfoRepository)
        {
            destination.Id = source.Id;
            destination.State = source.State;

            destination.Title = source.Title;
            destination.Description = source.Title;
            destination.SalaryMin = source.SalaryMin;
            destination.SalaryMax = source.SalaryMax;
            destination.TypeOfEmployment = source.TypeOfEmployment;
            destination.StartDate = source.StartDate;
            destination.EndDate = source.EndDate;
            destination.DeadlineDate = source.DeadlineDate;

            destination.ParentVacancyId = source.ParentVacancyId;
            destination.IndustryId = source.IndustryId;
            destination.DepartmentId = source.DepartmentId;
            destination.ResponsibleId = source.ResponsibleId;

            PerformLevelsSaving(destination, source, levelRepository);
            PerformLocationsSaving(destination, source, locationRepository);
            PerformTagsSaving(destination, source, tagRepository);
            PerformSkillsSaving(destination, source, skillRepository);
            PerformLanguageSkillsSaving(destination, source, languageSkillRepository);
            PerformVacanciesProgressSaving(destination, source, vacancyStageInfoRepository);
        }

        private static void PerformVacanciesProgressSaving(Vacancy destination, VacancyDTO source, IRepository<VacancyStageInfo> vacancyStageInfoRepository)
        {
            RefreshExistingVacanciesProgress(destination, source, vacancyStageInfoRepository);
            CreateNewVacanciesProgress(destination, source);
        }
        private static void CreateNewVacanciesProgress(Vacancy destination, VacancyDTO source)
        {
            source.CandidatesProgress.Where(x => x.Id == 0).ToList().ForEach(newVacancyStageInfo =>
            {
                var toDomain = new VacancyStageInfo();
                toDomain.Update(newVacancyStageInfo);
                if (toDomain.VacancyId == 0)
                {
                    toDomain.Vacancy = destination;
                }
                destination.CandidatesProgress.Add(toDomain);
            });
        }
        private static void RefreshExistingVacanciesProgress(Vacancy destination, VacancyDTO source, IRepository<VacancyStageInfo> vacancyStageInfoRepository)
        {
            source.CandidatesProgress.Where(x => x.Id != 0).ToList().ForEach(updatedVacanciesStageInfo =>
            {
                var domainVacancyStageInfo = destination.CandidatesProgress.FirstOrDefault(x => x.Id == updatedVacanciesStageInfo.Id);
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

        private static void PerformSkillsSaving(Vacancy destination, VacancyDTO source, IRepository<Skill> skillRepository)
        {
            destination.RequiredSkills.Clear();
            source.RequiredSkillIds.ToList().ForEach(skillId =>
            {
                destination.RequiredSkills.Add(skillRepository.Get(skillId));
            });
        }

        private static void PerformLocationsSaving(Vacancy destination, VacancyDTO source, IRepository<Location> locationRepository)
        {
            destination.Locations.Clear();
            source.LocationIds.ToList().ForEach(locationId =>
            {
                destination.Locations.Add(locationRepository.Get(locationId));
            });
        }

        private static void PerformTagsSaving(Vacancy destination, VacancyDTO source, IRepository<Tag> tagRepository)
        {
            destination.Tags.Clear();
            source.TagIds.ToList().ForEach(tagId =>
            {
                destination.Tags.Add(tagRepository.Get(tagId));
            });
        }

        private static void PerformLevelsSaving(Vacancy destination, VacancyDTO source, IRepository<Level> levelRepository)
        {
            destination.Levels.Clear();
            source.LevelIds.ToList().ForEach(levelId =>
            {
                destination.Levels.Add(levelRepository.Get(levelId));
            });
        }

        private static void PerformLanguageSkillsSaving(Vacancy destination, VacancyDTO source, IRepository<LanguageSkill> languageSkillRepository)
        {
            var updatedLanguageSkill = source.LanguageSkill;
            LanguageSkill domainLanguageSkill = destination.LanguageSkill;
            if (destination.LanguageSkill == null)
            {
                domainLanguageSkill = destination.LanguageSkill = new LanguageSkill();
            }
            if (updatedLanguageSkill == null)
            {
                throw new ArgumentNullException("Request contains unknown entity");
            }
            if (updatedLanguageSkill.ShouldBeRemoved())
            {
                languageSkillRepository.Remove(updatedLanguageSkill.Id);
            }
            else
            {
                domainLanguageSkill.Update(updatedLanguageSkill);
            }
        }
    }
}
