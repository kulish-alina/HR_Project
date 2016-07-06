using DAL.DTO;
using DAL.Infrastructure;
using Domain.Entities;
using Domain.Entities.Enum.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DAL.Extensions
{
    public static class VacancyExtension
    {
        public static void UpdateChildWithParent(this Vacancy childVacancy, Vacancy parentVacancy)
        {
            if (childVacancy.Id!=0 && childVacancy.ParentVacancyId != parentVacancy.Id)
            {
                throw new Exception("Child vacancy is not a child of specified parent vacancy");
            }
            childVacancy.State = parentVacancy.State;
            childVacancy.Title = parentVacancy.Title;
            childVacancy.Description = parentVacancy.Description;
            childVacancy.SalaryMin = parentVacancy.SalaryMin;
            childVacancy.SalaryMax = parentVacancy.SalaryMax;
            childVacancy.CurrencyId = parentVacancy.CurrencyId;
            childVacancy.TypeOfEmployment = parentVacancy.TypeOfEmployment;
            childVacancy.StartDate = parentVacancy.StartDate;
            childVacancy.EndDate = parentVacancy.EndDate;
            childVacancy.DeadlineDate = parentVacancy.DeadlineDate;
            childVacancy.IndustryId = parentVacancy.IndustryId;
            childVacancy.DepartmentId = parentVacancy.DepartmentId;
            childVacancy.ResponsibleId = parentVacancy.ResponsibleId;
            childVacancy.Levels = parentVacancy.Levels;
            childVacancy.Cities = parentVacancy.Cities;
            childVacancy.Tags = parentVacancy.Tags;
            childVacancy.RequiredSkills = parentVacancy.RequiredSkills;
            childVacancy.LanguageSkill = parentVacancy.LanguageSkill;
            childVacancy.CandidatesProgress = parentVacancy.CandidatesProgress;
            childVacancy.ParentVacancyId = parentVacancy.Id;
            childVacancy.CandidatesProgress.ToList().ForEach(x =>
            {
                x.Vacancy = childVacancy;
                x.VacancyId = childVacancy.Id;
            });
            childVacancy.Files = parentVacancy.Files;
            childVacancy.Comments = parentVacancy.Comments;
        }

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
            PerformAddingDeadlineToCalendar(destination, source, uow);
            destination.DeadlineToCalendar = source.DeadlineToCalendar;

            destination.ParentVacancyId = source.ParentVacancyId;
            destination.IndustryId = source.IndustryId;
            destination.DepartmentId = source.DepartmentId;
            destination.ResponsibleId = source.ResponsibleId;
            destination.CurrencyId = source.CurrencyId;
            destination.ChildVacanciesNumber = source.ChildVacanciesNumber;

            PerformLevelsSaving(destination, source, uow.LevelRepo);
            PerformLocationsSaving(destination, source, uow.CityRepo);
            PerformTagsSaving(destination, source, uow.TagRepo);
            PerformSkillsSaving(destination, source, uow.SkillRepo);
            PerformLanguageSkillsSaving(destination, source, uow.LanguageSkillRepo);
            PerformVacanciesProgressSaving(destination, source, uow.VacancyStageRepo);
            PerformFilesSaving(destination, source, uow.FileRepo);
            PerformCommentsSaving(destination, source, uow.CommentRepo);
            PerformChildVacanciesUpdating(destination);
        }

        private static void PerformAddingDeadlineToCalendar(Vacancy destination, VacancyDTO source, IUnitOfWork uow)
        {
            if (NeedAddDeadlineEvent(destination, source))
            {
                var eventType = uow.EventTypeRepo.Get(new List<Expression<Func<EventType, bool>>> { (x => x.Title.StartsWith("Vacancy deadline")) }).FirstOrDefault();
                if (!(eventType is EventType))
                {
                    throw new Exception("You should scpecify 'Vacancy deadline' event type");
                }
                uow.EventRepo.Insert(new Event
                {
                    EventDate = source.DeadlineDate.Value,
                    EventType = eventType,
                    ResponsibleId = source.ResponsibleId,
                    Vacancy = destination
                });
            }
            else if (NeedDeleteDeadlineEvent(destination, source))
            {
                var deadlineEvent = uow.EventRepo.Get(new List<Expression<Func<Event, bool>>> { x => x.EventType.Title.StartsWith("Vacancy deadline") && x.VacancyId == destination.Id }).FirstOrDefault();
                if (deadlineEvent is Event)
                {
                    uow.EventRepo.Delete(deadlineEvent);
                }
                else
                {
                    throw new Exception("Can not find event bounded to chosen vacancy");
                }
            }
        }

        private static bool NeedDeleteDeadlineEvent(Vacancy destination, VacancyDTO source)
        {
            return destination.DeadlineToCalendar && !source.DeadlineToCalendar;
        }

        private static bool NeedAddDeadlineEvent(Vacancy destination, VacancyDTO source)
        {
            return !destination.DeadlineToCalendar && source.DeadlineToCalendar;
        }

        private static void PerformChildVacanciesUpdating(Vacancy destination)
        {
            destination.ChildVacancies.ToList().ForEach(x =>
            {
                x.UpdateChildWithParent(destination);
            });
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

        private static void PerformLocationsSaving(Vacancy destination, VacancyDTO source, ICityRepository locationRepository)
        {
            destination.Cities.Clear();
            source.CityIds.ToList().ForEach(locationId =>
            {
                destination.Cities.Add(locationRepository.GetByID(locationId));
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
