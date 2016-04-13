using Domain.Entities;
using Domain.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using WebApi.DTO.DTOModels;
using WebApi.DTO.DTOService;

namespace WebApi.Controllers
{
    public abstract class BoTController<DomainEntity, ViewModel> : ApiController
        where DomainEntity : BaseEntity, new()
        where ViewModel : new()
    {
        protected IRepository<DomainEntity> _repo;
        JsonSerializerSettings BotJsonSerializerSettings { get; set; }

        [HttpGet]
        public virtual IHttpActionResult All()
        {
            var entities = _repo.GetAll().ToList();
            var dtoEntities = entities.Select(x => DTOService.ToDTO<DomainEntity, ViewModel>(x)).ToList();
            return Json(dtoEntities, BotJsonSerializerSettings);
        }

        [HttpGet]
        public virtual IHttpActionResult Get(int id)
        {
            var foundedEntity = _repo.Get(id);
            if (foundedEntity != null)
            {
                var foundedEntityDto = DTOService.ToDTO<DomainEntity, ViewModel>(foundedEntity);
                return Json(foundedEntityDto, BotJsonSerializerSettings);
            }
            return NotFound();
        }

        [HttpDelete]
        public virtual IHttpActionResult Delete(int id)
        {
            var foundedEntity = _repo.Get(id);
            if (foundedEntity != null)
            {
                _repo.Remove(foundedEntity);
                return Ok();
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPost]
        public virtual IHttpActionResult Add([FromBody]JObject entity)
        {
            var newEntityDto = entity.ToObject<ViewModel>();
            var newEntity = DTOService.ToEntity<ViewModel, DomainEntity>(newEntityDto);
            _repo.Add(newEntity);
            return Json(DTOService.ToDTO<DomainEntity, ViewModel>(_repo.GetAll().Last()));
        }

        [HttpPut]
        public virtual IHttpActionResult Put(int id, [FromBody]JObject entity)
        {
            var changedEntityDto = entity.ToObject<ViewModel>();
            if (changedEntityDto != null)
            {
                var changedEntity = DTOService.ToEntity<ViewModel, DomainEntity>(changedEntityDto);
                _repo.Update(changedEntity);
                return Json(DTOService.ToDTO<DomainEntity,ViewModel>(_repo.Get(changedEntity.Id)), BotJsonSerializerSettings);
            }
            return NotFound();
        }

        public BoTController()
        {
            BotJsonSerializerSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateFormatString = "yyyy-MM-dd"
            };
        }
        public StringContent SerializeContent(object obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj, Formatting.Indented, BotJsonSerializerSettings));
        }
    }
}