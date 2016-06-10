using BaseOfTalents.DAL.Infrastructure;
using BaseOfTalents.Domain.Entities;
using DAL.Infrastructure;
using Domain.DTO.DTOModels;
using System;
using System.Linq;

namespace DAL.Extensions
{
    public static class VacancyExtension
    {
        public static void Update(this Vacancy destination, VacancyDTO source, IUnitOfWork uow)
        {
            destination.Id = source.Id;
            destination.State = source.State;

            destination.Title = source.Title;
            destination.Description = source.Description;
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

            PerformLevelsSaving(destination, source, uow.LevelRepo);
            PerformLocationsSaving(destination, source, uow.LocationRepo);
            PerformTagsSaving(destination, source, uow.TagRepo);
            PerformSkillsSaving(destination, source, uow.SkillRepo);
            PerformLanguageSkillsSaving(destination, source, uow.LanguageSkillRepo);
            PerformVacanciesProgressSaving(destination, source, uow.VacancyStageRepo);
            PerformFilesSaving(destination, source, uow.FileRepo);
            PerformCommentsSaving(destination, source, uow.CommentRepo);
        }

        private static void PerformFilesSaving(Vacancy destination, VacancyDTO source, IFileRepository fileRepository)
        {
            source.Files.ToList().ForEach(file =>
            {
                var fileInVacancy = destination.Files.FirstOrDefault(x => x.Id == file.Id);
                var dbFile = fileRepository.GetByID(file.Id);
                if (dbFile == null)
                {
                    throw new Exception("Unknown file");
                }
                if (file.ShouldBeRemoved())
                {
                    fileRepository.Delete(file.Id);
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

        private static void PerformVacanciesProgressSaving(Vacancy destination, VacancyDTO source, IVacancyStageRepository vacancyStageInfoRepository)
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
        private static void RefreshExistingVacanciesProgress(Vacancy destination, VacancyDTO source, IVacancyStageRepository vacancyStageInfoRepository)
        {
            source.CandidatesProgress.Where(x => x.Id != 0).ToList().ForEach(updatedVacanciesStageInfo =>
            {
                var domainVacancyStageInfo = destination.CandidatesProgress.FirstOrDefault(x => x.Id == updatedVacanciesStageInfo.Id);
                if (domainVacancyStageInfo == null)
                {
                    throw new ArgumentNullException("You trying to update vacanies progress which is actually doesn't exists in database");
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
                    vacancyStageInfoRepository.Delete(updatedVacanciesStageInfo.Id);
                }
                else
                {
                    domainVacancyStageInfo.Update(updatedVacanciesStageInfo);
                }
            });
        }

        private static void PerformSkillsSaving(Vacancy destination, VacancyDTO source, ISkillRepository skillRepository)
        {
            destination.RequiredSkills.Clear();
            source.RequiredSkillIds.ToList().ForEach(skillId =>
            {
                destination.RequiredSkills.Add(skillRepository.GetByID(skillId));
            });
        }

        private static void PerformLocationsSaving(Vacancy destination, VacancyDTO source, ILocationRepository locationRepository)
        {
            destination.Locations.Clear();
            source.LocationIds.ToList().ForEach(locationId =>
            {
                destination.Locations.Add(locationRepository.GetByID(locationId));
            });
        }

        private static void PerformTagsSaving(Vacancy destination, VacancyDTO source, ITagRepository tagRepository)
        {
            destination.Tags.Clear();
            source.TagIds.ToList().ForEach(tagId =>
            {
                destination.Tags.Add(tagRepository.GetByID(tagId));
            });
        }

        private static void PerformLevelsSaving(Vacancy destination, VacancyDTO source, ILevelRepository levelRepository)
        {
            destination.Levels.Clear();
            source.LevelIds.ToList().ForEach(levelId =>
            {
                destination.Levels.Add(levelRepository.GetByID(levelId));
            });
        }

        private static void PerformLanguageSkillsSaving(Vacancy destination, VacancyDTO source, ILanguageSkillRepository languageSkillRepository)
        {
            var updatedLanguageSkill = source.LanguageSkill;
            LanguageSkill domainLanguageSkill = destination.LanguageSkill;
            if (destination.LanguageSkill == null)
            {
                domainLanguageSkill = destination.LanguageSkill = new LanguageSkill();
            }
            if (updatedLanguageSkill == null)
            {
                destination.LanguageSkill = null;
                return;
            }
            if (updatedLanguageSkill.ShouldBeRemoved())
            {
                languageSkillRepository.Delete(updatedLanguageSkill.Id);
            }
            else
            {
                domainLanguageSkill.Update(updatedLanguageSkill);
            }
        }
        private static void PerformCommentsSaving(Vacancy destination, VacancyDTO source, IRepository<Comment> commentRepository)
        {
            RefreshExistingComments(destination, source, commentRepository);
            CreateNewComments(destination, source);
        }
        private static void CreateNewComments(Vacancy destination, VacancyDTO source)
        {
            source.Comments.Where(x => x.IsNew()).ToList().ForEach(newComment =>
            {
                var toDomain = new Comment();
                toDomain.Update(newComment);
                destination.Comments.Add(toDomain);
            });
        }
        private static void RefreshExistingComments(Vacancy destination, VacancyDTO source, IRepository<Comment> commentRepository)
        {
            source.Comments.Where(x => !x.IsNew()).ToList().ForEach(updatedComment =>
            {
                var domainComment = destination.Comments.FirstOrDefault(x => x.Id == updatedComment.Id);
                if (domainComment == null)
                {
                    throw new ArgumentNullException("You trying to update comment which is actually doesn't exists in database");
                }
                if (updatedComment.ShouldBeRemoved())
                {
                    commentRepository.Delete(updatedComment.Id);
                }
                else
                {
                    domainComment.Update(updatedComment);
                }
            });
        }

    }
}
