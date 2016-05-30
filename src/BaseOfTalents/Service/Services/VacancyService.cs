using Domain.DTO.DTOModels;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Repositories;
using Domain.Entities.Enum.Setup;
using Domain.Entities.Setup;
using Service.Extentions;
using System.Data.Entity;

namespace Service.Services
{
    public class VacancyService : ControllerService<Vacancy, VacancyDTO>
    {
        IRepository<Level> levelRepository;
        IRepository<Location> locationRepository;
        IRepository<Skill> skillRepository;
        IRepository<Tag> tagRepository;
        IRepository<LanguageSkill> languageSkillRepository;
        IRepository<VacancyStageInfo> vacancyStageInfoRepository;
        IRepository<File> fileRepository;

        public VacancyService(
            IRepository<Vacancy> vacancyRepository, 
            IRepository<Level> levelRepository, 
            IRepository<Location> locationRepository, 
            IRepository<Skill> skillRepository, 
            IRepository<Tag> tagRepository,
            IRepository<LanguageSkill> languageSkillRepository,
            IRepository<VacancyStageInfo> vacancyStageInfoRepository,
            IRepository<File> fileRepository) : base(vacancyRepository)
        {
            this.levelRepository = levelRepository;
            this.locationRepository = locationRepository;
            this.skillRepository = skillRepository;
            this.tagRepository = tagRepository;
            this.languageSkillRepository = languageSkillRepository;
            this.vacancyStageInfoRepository = vacancyStageInfoRepository;
            this.fileRepository = fileRepository;
        }

        public override VacancyDTO Add(VacancyDTO vacancyToAdd)
        {
            Vacancy _vacancy = new Vacancy();

            _vacancy.Update(vacancyToAdd, 
                levelRepository, 
                locationRepository, 
                skillRepository, 
                tagRepository, 
                languageSkillRepository, 
                vacancyStageInfoRepository, 
                fileRepository); 

            entityRepository.Add(_vacancy);
            entityRepository.Commit();

            return DTOService.ToDTO<Vacancy, VacancyDTO>(_vacancy);
        }
        public override VacancyDTO Put(VacancyDTO entity)
        {
            Vacancy _vacancy = entityRepository.Get(entity.Id);

            _vacancy.Update(entity,
                levelRepository,
                locationRepository,
                skillRepository,
                tagRepository,
                languageSkillRepository,
                vacancyStageInfoRepository,
                fileRepository);

            entityRepository.Update(_vacancy);
            entityRepository.Commit();
            return DTOService.ToDTO<Vacancy, VacancyDTO>(_vacancy);
        }
        public override IEnumerable<VacancyDTO> GetAll()
        {
            return base.GetAll();
        }

        public override object Search(object searchParams)
        {
            VacancySearchParameters vacancySearchParams = searchParams as VacancySearchParameters;
            if (vacancySearchParams != null)
            {
                var vacanciesQuery = entityRepository.GetAll();

                var skipped = vacancySearchParams.Size * (vacancySearchParams.Current - 1);

                if (vacancySearchParams.IndustryId.HasValue)
                {
                    vacanciesQuery = vacanciesQuery.Where(x => x.IndustryId == vacancySearchParams.IndustryId);
                }
                if (vacancySearchParams.UserId.HasValue)
                {
                    vacanciesQuery = vacanciesQuery.Where(x => x.ResponsibleId == vacancySearchParams.UserId);
                }
                if (!String.IsNullOrWhiteSpace(vacancySearchParams.Title))
                {
                    vacanciesQuery = vacanciesQuery.Where(x => x.Title.StartsWith(vacancySearchParams.Title));
                }
                if (vacancySearchParams.LevelIds.Any())
                {
                    vacanciesQuery = vacanciesQuery
                    .Where(x => x.Levels.Any(l => vacancySearchParams.LevelIds.Contains(l.Id)));
                }
                if (vacancySearchParams.LocationIds.Any())
                {
                    vacanciesQuery = vacanciesQuery
                    .Where(x => x.Locations.Any(loc => vacancySearchParams.LocationIds.Contains(loc.Id)));
                }
                if (vacancySearchParams.TypeOfEmployment.HasValue)
                {
                    vacanciesQuery = vacanciesQuery.Where(x => x.TypeOfEmployment == vacancySearchParams.TypeOfEmployment);
                }
                if (vacancySearchParams.VacancyState.HasValue)
                {
                    vacanciesQuery = vacanciesQuery.Where(x => x.State == vacancySearchParams.VacancyState);
                }

                var entities = vacanciesQuery
                                        .AsNoTracking()
                                        .Paging(skipped, vacancySearchParams.Size)
                                       .ToList()
                                       .Select(x => DTOService.ToDTO<Vacancy, VacancyDTO>(x));
                return new { Vacancies = entities, Total = vacanciesQuery.Count(), Current = vacancySearchParams.Current };
            }
            return null;
        }
    }
}
