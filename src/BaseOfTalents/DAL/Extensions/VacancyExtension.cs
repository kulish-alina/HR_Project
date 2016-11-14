using DAL.DTO;
using DAL.Exceptions;
using DAL.Infrastructure;
using DAL.LoggerCore;
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
        public static void Update(this Vacancy destination, VacancyDTO source, IUnitOfWork uow, int userId)
        {
            LogChanges(destination, source, userId, uow);

            destination.Id = source.Id;
            if (source.Id == 0 || source.CloneVacanciesNumber.HasValue)
            {
                PerformVacancyStageFilling(destination, uow);
            }
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
            destination.ClosingCandidateId = source.ClosingCandidateId;
            PerformLevelsSaving(destination, source, uow.LevelRepo);
            PerformCitiesSaving(destination, source, uow.CityRepo);
            PerformTagsSaving(destination, source, uow.TagRepo);
            PerformSkillsSaving(destination, source, uow.SkillRepo);
            PerformLanguageSkillsSaving(destination, source, uow.LanguageSkillRepo);
            PerformVacanciesProgressSaving(destination, source, uow.VacancyStageInfoRepo);
            PerformFilesSaving(destination, source, uow.FileRepo);
            PerformCommentsSaving(destination, source, uow.CommentRepo);
            PerformVacancyStatesUpdating(destination, source);
        }

        private static void LogChanges(Vacancy destination, VacancyDTO source, int userId, IUnitOfWork uow)
        {
            Logger.Log(destination, source, userId, uow);
        }

        private static void PerformVacancyStatesUpdating(Vacancy destination, VacancyDTO source)
        {
            if (destination.State != source.State)
            {
                if (destination.StatesInfo.Any())
                {
                    destination.StatesInfo.Last().Passed = DateTime.Now;
                }
                destination.StatesInfo.Add(new VacancyState()
                {
                    CreatedOn = DateTime.Now,
                    State = source.State,
                    VacancyId = source.Id
                });
            }
            destination.State = source.State;
        }

        private static void PerformVacancyStageFilling(Vacancy destination, IUnitOfWork uow)
        {
            var stages = uow.StageRepo.Get(new List<Expression<Func<Stage, bool>>>() { x => x.IsDefault }).ToList();
            var extendedStages = stages.Select(x => new ExtendedStage { Stage = x, Order = x.Order }).ToList();
            extendedStages.ForEach(x => destination.StageFlow.Add(x));
            extendedStages = extendedStages.OrderBy(x => x.Order).ToList();
            destination.StageFlow = extendedStages;
        }

        private static void PerformAddingDeadlineToCalendar(Vacancy destination, VacancyDTO source, IUnitOfWork uow)
        {
            if (NeedAddDeadlineEvent(destination, source))
            {
                var eventType = uow.EventTypeRepo.Get(new List<Expression<Func<EventType, bool>>> { (x => x.Title.StartsWith("Vacancy deadline")) }).FirstOrDefault();
                if (!(eventType is EventType))
                {
                    throw new EntityNotFoundException("You should scpecify 'Vacancy deadline' event type");
                }
                uow.EventRepo.Insert(new Event
                {
                    EventDate = source.DeadlineDate.Value,
                    EventType = eventType,
                    ResponsibleId = source.ResponsibleId,
                    Vacancy = destination,
                    Description = string.Format("Deadline for {0} vacancy", destination.Title)
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
                    throw new EntityNotFoundException("Can not find event bounded to chosen vacancy");
                }
            }
            else
            {
                var deadlineEvent = uow.EventRepo.Get(new List<Expression<Func<Event, bool>>> { x => x.EventType.Title.StartsWith("Vacancy deadline") && x.VacancyId == destination.Id }).FirstOrDefault();
                if (deadlineEvent is Event)
                {
                    deadlineEvent.EventDate = source.DeadlineDate.Value;
                    uow.EventRepo.Update(deadlineEvent);
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

        private static void PerformVacanciesProgressSaving(Vacancy destination, VacancyDTO source, IVacancyStageInfoRepository vacancyStageInfoRepository)
        {

            //TODO: if vacancy id null - set to THIS
            RefreshExistingVacanciesProgress(destination, source, vacancyStageInfoRepository);
            CreateNewVacanciesProgress(destination, source);
        }
        private static void CreateNewVacanciesProgress(Vacancy destination, VacancyDTO source)
        {
            source.CandidatesProgress.Where(x => x.IsNew()).ToList().ForEach(newVacancyStageInfo =>
            {
                var toDomain = new VacancyStageInfo();
                toDomain.Update(destination, newVacancyStageInfo);
                destination.CandidatesProgress.Add(toDomain);
            });
        }
        private static void RefreshExistingVacanciesProgress(Vacancy destination, VacancyDTO source, IVacancyStageInfoRepository vacancyStageInfoRepository)
        {
            source.CandidatesProgress.Where(x => !x.IsNew()).ToList().ForEach(updatedVacanciesStageInfo =>
            {
                var domainVacancyStageInfo = destination.CandidatesProgress.FirstOrDefault(x => x.Id == updatedVacanciesStageInfo.Id);
                if (domainVacancyStageInfo == null)
                {
                    throw new ArgumentNullException("You trying to update vacanies progress which is actually doesn't exists in database");
                }
                if (updatedVacanciesStageInfo.ShouldBeRemoved())
                {
                    vacancyStageInfoRepository.Delete(updatedVacanciesStageInfo.Id);
                }
                else
                {
                    domainVacancyStageInfo.Update(destination, updatedVacanciesStageInfo);
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

        private static void PerformCitiesSaving(Vacancy destination, VacancyDTO source, ICityRepository locationRepository)
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
