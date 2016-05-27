using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Controllers
{
    public abstract class EnumController<TEnum> : ApiController
    {
        IEnumService<TEnum> EnumService { get; set; }

        public EnumController(IEnumService<TEnum> service)
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
        public virtual IHttpActionResult All()
        {
            var enums = EnumService.GetAll();
            return Json(enums, BOT_SERIALIZER_SETTINGS);
        }

        [HttpGet]
        // GET: api/Entities/
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