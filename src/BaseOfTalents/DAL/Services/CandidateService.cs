using BaseOfTalents.DAL.Infrastructure;
using BaseOfTalents.Domain.Entities;
using DAL.Extensions;
using Domain.DTO.DTOModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace DAL.Services
{
    public class CandidateService
    {
        IUnitOfWork uow;
        public CandidateService(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public CandidateDTO Get(int id)
        {
            var entity = uow.CandidateRepo.GetByID(id);
            return DTOService.ToDTO<Candidate, CandidateDTO>(entity);
        }

        public CandidateDTO Add(CandidateDTO candidateToAdd)
        {
            Candidate _candidate = new Candidate();
            _candidate.Update(candidateToAdd, uow);
            uow.CandidateRepo.Insert(_candidate);
            uow.Commit();
            return DTOService.ToDTO<Candidate, CandidateDTO>(_candidate);
        }

        public CandidateDTO Update(CandidateDTO entity)
        {
            Candidate _candidate = uow.CandidateRepo.GetByID(entity.Id);
            _candidate.Update(entity, uow);
            uow.CandidateRepo.Update(_candidate);
            uow.Commit();
            return DTOService.ToDTO<Candidate, CandidateDTO>(_candidate);
        }

        public Tuple<IEnumerable<CandidateDTO>, int> Get(
            string firstName,
            string lastName,
            bool? relocationAgreement,
            bool? isMale,
            int? minAge,
            int? maxAge,
            DateTime? startExperience,
            int? minSalary,
            int? maxSalary,
            int? currencyId,
            int? industryId,
            string position,
            string technology,
            IEnumerable<LanguageSkillDTO> languageSkills,
            IEnumerable<int> citiesIds,
            int current, int size)
        {
            var filters = new List<Expression<Func<Candidate, bool>>>();

            if (!String.IsNullOrEmpty(firstName))
            {
                filters.Add(x => x.FirstName.ToLower().StartsWith(firstName.ToLower()));
            }
            if (!String.IsNullOrEmpty(lastName))
            {
                filters.Add(x => x.LastName.ToLower().StartsWith(lastName.ToLower()));
            }
            if (relocationAgreement.HasValue)
            {
                filters.Add(x => x.RelocationAgreement == relocationAgreement.Value);
            }
            if (isMale.HasValue)
            {
                filters.Add(x => x.IsMale.Value == isMale.Value);
            }
            if (minAge.HasValue)
            {
                if (maxAge.HasValue)
                {
                    filters.Add(x => x.BirthDate.Value <= DbFunctions.AddYears(DateTime.Now, -minAge.Value) && x.BirthDate.Value >= DbFunctions.AddYears(DateTime.Now, -maxAge.Value));
                }
            }
            if (startExperience.HasValue)
            {
                filters.Add(x => x.StartExperience <= startExperience.Value);
            }
            if (minSalary.HasValue)
            {
                if (maxSalary.HasValue)
                {
                    filters.Add(x => x.SalaryDesired >= minSalary.Value && x.SalaryDesired <= maxSalary.Value);
                }
            }
            if (currencyId.HasValue)
            {
                filters.Add(x => x.CurrencyId == currencyId.Value);
            }
            if (industryId.HasValue)
            {
                filters.Add(x => x.IndustryId == industryId);
            }
            if (!String.IsNullOrEmpty(position))
            {
                filters.Add(cand => cand.PositionDesired.ToLower().StartsWith(position.ToLower()));
            }
            if (!String.IsNullOrEmpty(technology))
            {
                filters.Add(x => x.Skills.Any(s => s.Title.StartsWith(technology) || x.Tags.Any(t => t.Title.StartsWith(technology))));
            }
            if (citiesIds.Any())
            {
                filters.Add(x => citiesIds.Any(y => y == x.CityId));
            }
            if (languageSkills.Any())
            {
                foreach (var ls in languageSkills)
                {
                    if (ls.LanguageLevel.HasValue)
                    {
                        filters.Add(x => x.LanguageSkills.Any(l => l.LanguageId == ls.LanguageId && l.LanguageLevel.Value >= ls.LanguageLevel.Value));
                    }
                    else
                    {
                        filters.Add(x => x.LanguageSkills.Any(l => l.LanguageId == ls.LanguageId));
                    }
                }
            }
            var candidates = uow.CandidateRepo.Get(filters);
            var total = candidates.Count();

            return new Tuple<IEnumerable<CandidateDTO>, int>(
                candidates.Skip(current * size).Take(size).Select(candidate => DTOService.ToDTO<Candidate, CandidateDTO>(candidate)),
                total);
        }


        public bool Delete(int id)
        {
            bool deleteResult;
            var entityToDelete = uow.CandidateRepo.GetByID(id);
            if (entityToDelete == null)
            {
                deleteResult = false;
            }
            else {
                uow.CandidateRepo.Delete(id);
                uow.Commit();
                deleteResult = true;
            }
            return deleteResult;
        }
    }
}
