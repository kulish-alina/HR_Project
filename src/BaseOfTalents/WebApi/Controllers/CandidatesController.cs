using Data.EFData.Extentions;
using Data.Infrastructure;
using Domain.DTO.DTOModels;
using Domain.Entities;
using Domain.Entities.Enum.Setup;
using Domain.Entities.Setup;
using Domain.Repositories;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using WebApi.DTO.DTOService;

namespace WebApi.Controllers
{
    public class CandidatesController : BoTController<Candidate, CandidateDTO>
    {
        public CandidatesController(IDataRepositoryFactory repoFactory, IErrorRepository errorRepo)
            : base(repoFactory, errorRepo)
        {
        }

        public CandidatesController()
        {
        }

        [HttpGet]
        [Route("api/Candidates/")]
        public IHttpActionResult All(HttpRequestMessage request, int? pageNumber = 1)
        {
            return this.PagedEntities(request, pageNumber);
        }

        [HttpGet]
        // GET: api/Entities/
        [Route("api/Candidates{pageNumber:int}")]
        public IHttpActionResult PagedEntities(HttpRequestMessage request, int? pageNumber = 1)
        {
            var _currentRepo = _repoFactory.GetDataRepository<Candidate>(request);
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
                                        .Select(x => DTOService.ToDTO<Candidate, CandidateDTO>(x));
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

        public override IHttpActionResult Add(HttpRequestMessage request, [FromBody]CandidateDTO candidate)
        {
            var _candidateRepo = _repoFactory.GetDataRepository<Candidate>(request);

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
                    if (candidate.Id != 0)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        Candidate _candidate = new Candidate();
                        _candidate.Update(candidate,
                            _repoFactory.GetDataRepository<Skill>(request),
                            _repoFactory.GetDataRepository<Tag>(request),
                            _repoFactory.GetDataRepository<CandidateSocial>(request),
                            _repoFactory.GetDataRepository<LanguageSkill>(request),
                            _repoFactory.GetDataRepository<CandidateSource>(request),
                            _repoFactory.GetDataRepository<VacancyStageInfo>(request),
                            _repoFactory.GetDataRepository<PhoneNumber>(request),
                            _repoFactory.GetDataRepository<Photo>(request),
                            _repoFactory.GetDataRepository<Vacancy>(request));
                        _candidateRepo.Add(_candidate);
                        _candidateRepo.Commit();
                        return Json(DTOService.ToDTO<Candidate, CandidateDTO>(_candidate), BOT_SERIALIZER_SETTINGS);
                    }
                }
            });
        }

        public override IHttpActionResult Put(HttpRequestMessage request, int id, [FromBody] CandidateDTO changedEntity)
        {
            var _candidateRepo = _repoFactory.GetDataRepository<Candidate>(request);

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
                        Candidate _candidate = _candidateRepo.Get(id);
                        _candidate.Update(changedEntity,
                            _repoFactory.GetDataRepository<Skill>(request),
                            _repoFactory.GetDataRepository<Tag>(request),
                            _repoFactory.GetDataRepository<CandidateSocial>(request),
                            _repoFactory.GetDataRepository<LanguageSkill>(request),
                            _repoFactory.GetDataRepository<CandidateSource>(request),
                            _repoFactory.GetDataRepository<VacancyStageInfo>(request),
                            _repoFactory.GetDataRepository<PhoneNumber>(request),
                            _repoFactory.GetDataRepository<Photo>(request),
                            _repoFactory.GetDataRepository<Vacancy>(request));
                        _candidateRepo.Update(_candidate);
                        _candidateRepo.Commit();
                        return Json(DTOService.ToDTO<Candidate, CandidateDTO>(_candidate), BOT_SERIALIZER_SETTINGS);
                    }
                }
            });
        }
    }
}