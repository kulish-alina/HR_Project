using Data.EFData.Extentions;
using Data.Infrastructure;
using Domain.DTO.DTOModels;
using Domain.Entities;
using Domain.Entities.Enum.Setup;
using Domain.Entities.Setup;
using Domain.Repositories;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using WebApi.DTO.DTOService;

namespace WebApi.Controllers
{
    public class VacanciesController : BoTController<Vacancy, VacancyDTO>
    {
        public VacanciesController(IDataRepositoryFactory repoFatory, IErrorRepository errorRepo)
            : base(repoFatory, errorRepo)
        {
        }

        public VacanciesController()
        {
        }

        public override IHttpActionResult Add(HttpRequestMessage request, [FromBody]VacancyDTO vacancy)
        {
            var _vacancyRepo = _repoFactory.GetDataRepository<Vacancy>(request);

            return CreateResponse(request, () =>
            {
                if (!ModelState.IsValid)
                {
                    StringBuilder errorString = new StringBuilder();
                    foreach (var error in ModelState.Keys.SelectMany(k => ModelState[k].Errors))
                    {
                        errorString.Append(error.ErrorMessage + '\n');
                    }
                    return BadRequest(errorString.ToString());
                }
                else
                {
                    if (vacancy.Id != 0)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        Vacancy _vacancy = new Vacancy();
                        _vacancy.Update(vacancy,
                            _repoFactory.GetDataRepository<Level>(request),
                            _repoFactory.GetDataRepository<Location>(request),
                            _repoFactory.GetDataRepository<Skill>(request),
                            _repoFactory.GetDataRepository<Tag>(request),
                            _repoFactory.GetDataRepository<LanguageSkill>(request),
                            _repoFactory.GetDataRepository<VacancyStageInfo>(request));
                        _vacancyRepo.Add(_vacancy);
                        _vacancyRepo.Commit();
                        return Json(DTOService.ToDTO<Vacancy, VacancyDTO>(_vacancy), BOT_SERIALIZER_SETTINGS);
                    }
                }
            });
        }

        public override IHttpActionResult All(HttpRequestMessage request)
        {
            return this.All(request);
        }

        public override IHttpActionResult Put(HttpRequestMessage request, int id, [FromBody] VacancyDTO changedEntity)
        {
            var _vacancyRepository = _repoFactory.GetDataRepository<Vacancy>(request);

            return CreateResponse(request, () =>
            {
                if (!ModelState.IsValid)
                {
                    StringBuilder errorString = new StringBuilder();
                    foreach (var error in ModelState.Keys.SelectMany(k => ModelState[k].Errors))
                    {
                        errorString.Append(error.ErrorMessage + '\n');
                    }
                    return BadRequest(errorString.ToString());
                }
                else
                {
                    if (changedEntity.Id != id)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        Vacancy _vacancy = _vacancyRepository.Get(id);
                        _vacancy.Update(changedEntity,
                            _repoFactory.GetDataRepository<Level>(request),
                            _repoFactory.GetDataRepository<Location>(request),
                            _repoFactory.GetDataRepository<Skill>(request),
                            _repoFactory.GetDataRepository<Tag>(request),
                            _repoFactory.GetDataRepository<LanguageSkill>(request),
                            _repoFactory.GetDataRepository<VacancyStageInfo>(request));
                        _vacancyRepository.Update(_vacancy);
                        _vacancyRepository.Commit();
                        return Json(DTOService.ToDTO<Vacancy, VacancyDTO>(_vacancy), BOT_SERIALIZER_SETTINGS);
                    }
                }
            });
        }

        [HttpPost]
        [Route("api/vacancies/search")]
        public IHttpActionResult Search(HttpRequestMessage request, [FromBody]VacancySearchParameters searchParams)
        {
            var _vacancyRepository = _repoFactory.GetDataRepository<Vacancy>(request);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return CreateResponse(request, () =>
            {
                var vacanciesQuery = _vacancyRepository.GetAll();

                var skipped = searchParams.Size * (searchParams.Current - 1);

                if (searchParams.IndustryId.HasValue)
                {
                    vacanciesQuery = vacanciesQuery.Where(x => x.IndustryId == searchParams.IndustryId);
                }
                if (searchParams.UserId.HasValue)
                {
                    vacanciesQuery = vacanciesQuery.Where(x => x.ResponsibleId == searchParams.UserId);
                }
                if (!String.IsNullOrWhiteSpace(searchParams.Title))
                {
                    vacanciesQuery = vacanciesQuery.Where(x => x.Title.StartsWith(searchParams.Title));
                }
                if (searchParams.LevelIds.Any())
                {
                    vacanciesQuery = vacanciesQuery
                    .Where(x => x.Levels.Any(l => searchParams.LevelIds.Contains(l.Id)));
                }
                if (searchParams.LocationIds.Any())
                {
                    vacanciesQuery = vacanciesQuery
                    .Where(x => x.Locations.Any(loc => searchParams.LocationIds.Contains(loc.Id)));
                }
                if (searchParams.TypeOfEmployment.HasValue)
                {
                    vacanciesQuery = vacanciesQuery.Where(x => x.TypeOfEmployment == searchParams.TypeOfEmployment);
                }
                if (searchParams.VacancyState.HasValue)
                {
                    vacanciesQuery = vacanciesQuery.Where(x => x.State == searchParams.VacancyState);
                }

                var entities = vacanciesQuery
                                        .AsNoTracking()
                                        .Paging(skipped,searchParams.Size)
                                       .ToList()
                                       .Select(x => DTOService.ToDTO<Vacancy, VacancyDTO>(x));
                return Json(new { Vacancies = entities, Total = vacanciesQuery.Count(), Current = searchParams.Current }, BOT_SERIALIZER_SETTINGS);
            });
        }

        [HttpGet]
        // GET: api/Entities/
        [Route("api/{controller}{pageNumber:int}")]
        private IHttpActionResult All(HttpRequestMessage request, int? pageNumber = 1)
        {
            var _currentRepo = _repoFactory.GetDataRepository<Vacancy>(request);
            return CreateResponse(request, () =>
            {
                var entitiesQuery = _currentRepo.GetAll().OrderBy(x => x.Id).ToList();
                if (pageNumber.HasValue)
                {
                    var totalCount = _currentRepo.GetAll().Count();
                    var totalPages = Math.Ceiling((double)totalCount / ENTITIES_PER_PAGE);
                    var entities = entitiesQuery
                                        .Skip((pageNumber.Value - 1) * ENTITIES_PER_PAGE)
                                        .Take(ENTITIES_PER_PAGE)
                                        .ToList()
                                        .Select(x => DTOService.ToDTO<Vacancy, VacancyDTO>(x));
                    return Json(new
                    {
                        totalCount = totalCount,
                        totalPages = totalPages,
                        queryResult = entities
                    }, BOT_SERIALIZER_SETTINGS);
                }
                return BadRequest();
            });
        }
    }
}