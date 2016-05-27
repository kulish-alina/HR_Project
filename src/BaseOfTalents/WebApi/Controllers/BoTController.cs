using Data.Infrastructure;
using Service.Services;
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

namespace WebApi.Controllers
{
    public abstract class BoTController<DomainEntity, ViewModel> : ApiController
        where DomainEntity : BaseEntity, new()
        where ViewModel : BaseEntityDTO, new()
    {
        protected IControllerService<DomainEntity, ViewModel> entityService;

        protected static JsonSerializerSettings BOT_SERIALIZER_SETTINGS = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public BoTController(IControllerService<DomainEntity, ViewModel> service)
        {
            entityService = service;
        }

        public BoTController()
        {
        }

        [HttpGet]
        public virtual IHttpActionResult Get()
        {
            var foundedEntities = entityService.GetAll();
            return Json(foundedEntities, BOT_SERIALIZER_SETTINGS);
        }

        [HttpGet]
        public virtual IHttpActionResult Get(int id)
        {
            var foundedEntity = entityService.GetById(id);
            if (foundedEntity != null)
            {
                return Json(foundedEntity, BOT_SERIALIZER_SETTINGS);
            }
            return NotFound();
        }

        [HttpDelete]
        public virtual IHttpActionResult Remove(int id)
        {
            try 
            {
                entityService.Remove(id);
                return Ok();
            }
            catch(MissingMemberException)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
        }

        [HttpPost]
        public virtual IHttpActionResult Add([FromBody]ViewModel entity)
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
            var newEntity = entityService.Add(entity);
            return Json((newEntity), BOT_SERIALIZER_SETTINGS);
        }

        [HttpPut]
        public virtual IHttpActionResult Put(int id, [FromBody]ViewModel changedEntity)
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
            var domainChangedEntity = entityService.Put(changedEntity);
            return Json(domainChangedEntity, BOT_SERIALIZER_SETTINGS);
        }

        /*protected IHttpActionResult CreateResponse(HttpRequestMessage request, Func<IHttpActionResult> function)
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
        }*/
    }
}