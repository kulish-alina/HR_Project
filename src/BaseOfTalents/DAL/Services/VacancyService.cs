using DAL.DTO;
using DAL.Extensions;
using DAL.Infrastructure;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DAL.Services
{
    public partial class VacancyService
    {
        IUnitOfWork uow;
        public VacancyService(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public VacancyDTO Get(int id)
        {
            var entity = uow.VacancyRepo.GetByID(id);

            return DTOService.ToDTO<Vacancy, VacancyDTO>(entity);
        }

        public Tuple<IEnumerable<VacancyDTO>, int> Get(
            int? userId,
            int? industryId,
            string title,
            int? state,
            int? typeOfEmployment,
            IEnumerable<int> levelIds,
            IEnumerable<int> locationIds,
            int current,
            int size,
            string sortBy,
            bool? sortAsc,
            int? departmentId
            )
        {
            var filters = new List<Expression<Func<Vacancy, bool>>>();

            if (userId.HasValue)
            {
                filters.Add(x => x.Responsible.Id == userId);
            }
            if (industryId.HasValue)
            {
                filters.Add(x => x.IndustryId == industryId);
            }
            if (!String.IsNullOrEmpty(title))
            {
                filters.Add(x => x.Title.ToLower().Contains(title.ToLower()));
            }
            if (state.HasValue)
            {
                filters.Add(x => (int)x.State == state);
            }
            if (typeOfEmployment.HasValue)
            {
                filters.Add(x => (int)x.TypeOfEmployment == typeOfEmployment);
            }
            if (levelIds.Any())
            {
                filters.Add(x => x.Levels.Any(l => levelIds.Contains(l.Id)));
            }
            if (locationIds.Any())
            {
                filters.Add(x => x.Cities.Any(loc => locationIds.Contains(loc.Id)));
            }
            if (departmentId.HasValue)
            {
                filters.Add(x => x.Department.Id == departmentId);
            }
            var vacancies = uow.VacancyRepo.Get(filters);

            var orderBy = sortBy ?? "Title";
            var sortAscend = sortAsc ?? true;

            if (typeof(Vacancy).GetProperty(orderBy) != null)
            {
                Func<Vacancy, object> keySelector = v =>
                {
                    switch (orderBy)
                    {
                        case "Cities":
                            return v.Cities.Last().Title;
                        case "Department":
                            return v.Department.Title;
                        case "Responsible":
                            return v.Responsible.LastName + "|" + v.Responsible.FirstName;
                        default:
                            return v.GetType().GetProperty(orderBy).GetValue(v);
                    }
                }; 
                vacancies = sortAscend ?
                    vacancies.OrderBy(keySelector) :
                    vacancies.OrderByDescending(keySelector);
            }

            var total = vacancies.Count();

            return new Tuple<IEnumerable<VacancyDTO>, int>(
                vacancies.Skip((current - 1) * size).Take(size).Select(vacancy => DTOService.ToDTO<Vacancy, VacancyDTO>(vacancy)),
                total);
        }

        public VacancyDTO Update(VacancyDTO vacancy, int userId)
        {
            var vacancyToUpdate = uow.VacancyRepo.GetByID(vacancy.Id);
            vacancyToUpdate.Update(vacancy, uow, userId);
            var clones = CloneVacanciesIfNeeded(vacancyToUpdate, vacancy, userId);
            clones.ForEach(clone =>
            {
                uow.VacancyRepo.Insert(clone);
            });
            uow.VacancyRepo.Update(vacancyToUpdate);
            uow.Commit();
            var super = uow.VacancyRepo.GetByID(vacancyToUpdate.Id);
            return DTOService.ToDTO<Vacancy, VacancyDTO>(super);
        }

        public VacancyDTO Add(VacancyDTO vacancy, int userId)
        {
            var vacancyToAdd = new Vacancy();
            vacancyToAdd.Update(vacancy, uow, userId);
            var clones = CloneVacanciesIfNeeded(vacancyToAdd, vacancy, userId);
            clones.ForEach(clone =>
            {
                uow.VacancyRepo.Insert(clone);
            });
            uow.VacancyRepo.Insert(vacancyToAdd);
            uow.Commit();
            return DTOService.ToDTO<Vacancy, VacancyDTO>(vacancyToAdd);
        }

        public bool Delete(int id)
        {
            bool deleteResult;
            var entityToDelete = uow.VacancyRepo.GetByID(id);
            if (entityToDelete == null)
            {
                deleteResult = false;
            }
            else
            {
                uow.VacancyRepo.Delete(id);
                uow.Commit();
                deleteResult = true;
            }
            return deleteResult;
        }

        private List<Vacancy> CloneVacanciesIfNeeded(Vacancy domain, VacancyDTO dto, int userId)
        {
            List<Vacancy> clonedVacancies = new List<Vacancy>();
            if (dto.CloneVacanciesNumber.HasValue)
            {
                var countOfClones = dto.CloneVacanciesNumber.Value - 1;
                while (countOfClones-- != 0)
                {
                    var clone = new Vacancy();
                    dto.CandidatesProgress = new List<VacancyStageInfoDTO>();
                    dto.Comments = new List<CommentDTO>();
                    clone.Update(dto, uow, userId);
                    clone.Id = 0;
                    clonedVacancies.Add(clone);
                }
            }
            return clonedVacancies;
        }
    }
}