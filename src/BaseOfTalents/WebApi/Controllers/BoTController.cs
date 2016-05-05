using Data.Infrastructure;
using Domain.Entities;
using Domain.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Results;
using WebApi.DTO.DTOService;

namespace WebApi.Controllers
{
    public abstract class BoTController<DomainEntity, ViewModel> : ApiController
        where DomainEntity : BaseEntity, new()
        where ViewModel : new()
    {
        protected IDataRepositoryFactory _repoFactory;
        protected IErrorRepository _errorRepository;

        protected readonly IUnitOfWork _unitOfWork;
        protected static int ENTITIES_PER_PAGE = 30;
        protected static JsonSerializerSettings BOT_SERIALIZER_SETTINGS = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
        
        public BoTController(IDataRepositoryFactory repoFatory, IUnitOfWork unitOfWork, IErrorRepository errorRepo)
        {
            _repoFactory = repoFatory;
            _unitOfWork = unitOfWork;
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
                    _unitOfWork.Commit();
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
                    _unitOfWork.Commit();
                    return Json(DTOService.ToDTO<DomainEntity, ViewModel>(_currentRepo.GetAll().OrderByDescending(x => x.Id).First()), BOT_SERIALIZER_SETTINGS);
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
                    _unitOfWork.Commit();
                    return Json(DTOService.ToDTO<DomainEntity, ViewModel>(_currentRepo.Get(changedDomainEntity.Id)), BOT_SERIALIZER_SETTINGS);
                }
            });
        }

        protected IHttpActionResult CreateResponse(HttpRequestMessage request, Func<IHttpActionResult> function)
        {
            IHttpActionResult response = null;
            try
            {
                response = function.Invoke();
            }
            catch (DbUpdateException ex)
            {
                LogError(ex);
                response = BadRequest(ex.InnerException.Message);
            }
            catch (Exception ex)
            {
                LogError(ex);
                response = InternalServerError(ex);
            }
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
                _unitOfWork.Commit();
            }
            catch { }
        }
    }
}