using DAL.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Web.Http;

namespace WebApi.Controllers
{
    public abstract class BaseEnumController<TEnum> : ApiController
    {
        protected BaseEnumService<TEnum> EnumService;

        public BaseEnumController(BaseEnumService<TEnum> service)
        {
            EnumService = service;
        }

        protected static JsonSerializerSettings BOT_SERIALIZER_SETTINGS = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        [HttpGet]
        // GET: api/Entities/
        [Route("")]
        public virtual IHttpActionResult All()
        {
            var enums = EnumService.GetAll();
            return Json(enums, BOT_SERIALIZER_SETTINGS);
        }

        [HttpGet]
        // GET: api/Entities/
        [Route("{id:int}")]
        public virtual IHttpActionResult Get(int id)
        {
            var foundedEnum = EnumService.Get(id);
            if (foundedEnum == null)
            {
                return BadRequest();
            }
            return Json(foundedEnum, BOT_SERIALIZER_SETTINGS);
        }
    }
}