using DAL.DTO;
using DAL.Services;
using Domain.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Web.Http;

namespace WebUI.Controllers
{
    public abstract class BaseController<DomainEntity, ViewModel> : ApiController
        where DomainEntity : BaseEntity, new()
        where ViewModel : BaseEntityDTO, new()
    {
        protected BaseService<DomainEntity, ViewModel> entityService;

        protected static JsonSerializerSettings BOT_SERIALIZER_SETTINGS = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public BaseController(BaseService<DomainEntity, ViewModel> service)
        {
            entityService = service;
        }

        public BaseController()
        {
        }

        [HttpGet]
        [Route("")]
        public virtual IHttpActionResult Get()
        {
            var foundedEntities = entityService.Get();
            return Json(foundedEntities, BOT_SERIALIZER_SETTINGS);
        }

        [HttpGet]
        [Route("{id:int}")]
        public virtual IHttpActionResult Get(int id)
        {
            var foundedEntity = entityService.Get(id);
            if (foundedEntity == null)
            {
                ModelState.AddModelError("Entity", "Entity with id " + id + " not founded");
                return BadRequest(ModelState);
            }
            return Json(foundedEntity, BOT_SERIALIZER_SETTINGS);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public virtual IHttpActionResult Remove(int id)
        {
            if (entityService.Delete(id))
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("")]
        public virtual IHttpActionResult Add([FromBody]ViewModel entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newEntity = entityService.Add(entity);
            return Json((newEntity), BOT_SERIALIZER_SETTINGS);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual IHttpActionResult Put(int id, [FromBody]ViewModel changedEntity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var domainChangedEntity = entityService.Update(changedEntity);
            return Json(domainChangedEntity, BOT_SERIALIZER_SETTINGS);
        }
    }
}