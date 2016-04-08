using Domain.Entities;
using Domain.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.DTO.DTOService.Abstract;

namespace WebApi.Controllers
{
    public class BoTController<TEntity, YEntity> : ApiController
        where TEntity : BaseEntity, new()
        where YEntity : new()
    {
        protected IRepository<TEntity> _repo;
        protected IDTOService<TEntity, YEntity> _dtoService;
        public JsonSerializerSettings BotJsonSerializerSettings { get; set; }

        [HttpGet]
        public virtual HttpResponseMessage All()
        {
            var entities = _repo.GetAll().ToList();
            var dtoEntities = entities.Select(x => _dtoService.ToDTO(x)).ToList();
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
                var foundedEntityDto = _dtoService.ToDTO(foundedEntity);
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
            var newEntityDto = entity.ToObject<YEntity>();
            var newEntity = _dtoService.ToEntity(newEntityDto);
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
            var changedEntityDto = entity.ToObject<YEntity>();
            HttpResponseMessage response = new HttpResponseMessage();
            if (changedEntityDto != null)
            {
                var changedEntity = _dtoService.ToEntity(changedEntityDto);
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