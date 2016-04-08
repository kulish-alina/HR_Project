using Domain.Entities;
using Domain.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
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
        public virtual HttpResponseMessage All()
        {
            var entities = _repo.GetAll().ToList();
            var dtoEntities = entities.Select(x => DTOService.ToDTO<DomainEntity, ViewModel>(x)).ToList();
            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = SerializeContent(dtoEntities)
            };
        }

        [HttpGet]
        public virtual HttpResponseMessage Get(int id)
        {
            HttpResponseMessage response;
            var foundedEntity = _repo.Get(id);
            if (foundedEntity != null)
            {
                var foundedEntityDto = DTOService.ToDTO<DomainEntity, ViewModel>(foundedEntity);
                response = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = SerializeContent(foundedEntityDto)
                };
            }
            else
            {
                response = new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            return response;
        }


        [HttpDelete]
        public virtual HttpResponseMessage Delete(int id)
        {
            HttpResponseMessage response;
            var foundedEntity = _repo.Get(id);
            if (foundedEntity != null)
            {
                _repo.Remove(foundedEntity);
                response = new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                response = new HttpResponseMessage(HttpStatusCode.NoContent);
            }
            return response;
        }

        [HttpPost]
        public virtual HttpResponseMessage Add([FromBody]JObject entity)
        {
            var newEntityDto = entity.ToObject<ViewModel>();
            var newEntity = DTOService.ToEntity<ViewModel, DomainEntity>(newEntityDto);
            _repo.Add(newEntity);
            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.Created,
                Content = SerializeContent(_repo.GetAll().Last())
            };
        }

        [HttpPut]
        public virtual HttpResponseMessage Put(int id, [FromBody]JObject entity)
        {
            var changedEntityDto = entity.ToObject<ViewModel>();
            HttpResponseMessage response = new HttpResponseMessage();
            if (changedEntityDto != null)
            {
                var changedEntity = DTOService.ToEntity<ViewModel, DomainEntity>(changedEntityDto);
                _repo.Update(changedEntity);
                response.StatusCode = HttpStatusCode.OK;
            }
            else
            {
                response.StatusCode = HttpStatusCode.NotFound;
            }
            return response;
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