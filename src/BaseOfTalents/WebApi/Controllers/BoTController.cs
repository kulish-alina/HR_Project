using Data.Infrastructure;
using Domain.DTO.DTOModels;
using Domain.Entities;
using Domain.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.DTO.DTOService;

namespace WebApi.Controllers
{
    public abstract class BoTController<DomainEntity, ViewModel> : ApiController
        where DomainEntity : BaseEntity, new()
        where ViewModel : BaseEntityDTO, new()
    {
        protected IDataRepositoryFactory _repoFactory;
        protected IErrorRepository _errorRepository;

        protected static int ENTITIES_PER_PAGE = 30;

        protected static JsonSerializerSettings BOT_SERIALIZER_SETTINGS = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public BoTController(IDataRepositoryFactory repoFatory, IErrorRepository errorRepo)
        {
            _repoFactory = repoFatory;
            _errorRepository = errorRepo;
        }

        public BoTController()
        {
        }

        [HttpGet]
        // GET: api/Entities/
        [Route("api/{controller}")]
        public virtual IHttpActionResult All(HttpRequestMessage request)
        {
            var _currentRepo = _repoFactory.GetDataRepository<DomainEntity>(request);
            return CreateResponse(request, () =>
            {
                var entitiesQuery = _currentRepo.GetAll().OrderBy(x => x.Id);

                var entities = entitiesQuery
                                    .ToList()
                                    .Select(x => DTOService.ToDTO<DomainEntity, ViewModel>(x));
                return Json(entities, BOT_SERIALIZER_SETTINGS);
            });
        }

        [HttpGet]
        public virtual IHttpActionResult Get(HttpRequestMessage request, int id)
        {
            var _currentRepo = _repoFactory.GetDataRepository<DomainEntity>(request);
            return CreateResponse(request, () =>
            {
                var foundedEntity = _currentRepo.Get(id);
                if (foundedEntity != null)
                {
                    var foundedEntityDto = DTOService.ToDTO<DomainEntity, ViewModel>(foundedEntity);
                    return Json(foundedEntityDto, BOT_SERIALIZER_SETTINGS);
                }
                return NotFound();
            });
        }

        [HttpDelete]
        public virtual IHttpActionResult Remove(HttpRequestMessage request, int id)
        {
            var _currentRepo = _repoFactory.GetDataRepository<DomainEntity>(request);
            return CreateResponse(request, () =>
            {
                var entityToRemove = _currentRepo.Get(id);
                if (entityToRemove != null)
                {
                    _currentRepo.Remove(entityToRemove);
                    _currentRepo.Commit();
                    return Ok();
                }
                return StatusCode(HttpStatusCode.NoContent);
            });
        }

        [HttpPost]
        public virtual IHttpActionResult Add(HttpRequestMessage request, [FromBody]ViewModel entity)
        {
            var _currentRepo = _repoFactory.GetDataRepository<DomainEntity>(request);
            return CreateResponse(request, () =>
            {
                if (!ModelState.IsValid)
                {
                    var errorList = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(x => x.ErrorMessage);

                    return Json(new
                    {
                        summary = "Bad request",
                        errorList = errorList
                    });
                }
                else
                {
                    var newEntity = DTOService.ToEntity<ViewModel, DomainEntity>(entity);
                    _currentRepo.Add(newEntity);
                    _currentRepo.Commit();
                    return Json(DTOService.ToDTO<DomainEntity, ViewModel>(newEntity), BOT_SERIALIZER_SETTINGS);
                }
            });
        }

        [HttpPut]
        public virtual IHttpActionResult Put(HttpRequestMessage request, int id, [FromBody]ViewModel changedEntity)
        {
            var _currentRepo = _repoFactory.GetDataRepository<DomainEntity>(request);

            return CreateResponse(request, () =>
            {
                if (!ModelState.IsValid)
                {
                    var errorList = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(x => x.ErrorMessage);

                    return Json(new
                    {
                        summary = "Bad request",
                        errorList = errorList
                    });
                }
                else
                {
                    var changedDomainEntity = DTOService.ToEntity<ViewModel, DomainEntity>(changedEntity);
                    _currentRepo.Update(changedDomainEntity);
                    _currentRepo.Commit();
                    return Json(DTOService.ToDTO<DomainEntity, ViewModel>(changedDomainEntity), BOT_SERIALIZER_SETTINGS);
                }
            });
        }

        protected IHttpActionResult CreateResponse(HttpRequestMessage request, Func<IHttpActionResult> function)
        {
            IHttpActionResult response = null;
            response = function.Invoke();
            return response;
        }

        private void LogError(Exception ex)
        {
            try
            {
                Error _error = new Error()
                {
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                };
                _errorRepository.Add(_error);
                _errorRepository.Commit();
            }
            catch { }
        }
    }
}