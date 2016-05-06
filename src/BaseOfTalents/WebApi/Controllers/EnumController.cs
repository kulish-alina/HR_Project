using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Controllers
{
    public abstract class EnumController<TEnum> : ApiController
    {
        protected static JsonSerializerSettings BOT_SERIALIZER_SETTINGS = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        [HttpGet]
        // GET: api/Entities/
        public virtual IHttpActionResult All(HttpRequestMessage request)
        {
            List<TEnum> enums = new List<TEnum>();
            foreach (var item in Enum.GetValues(typeof(TEnum)))
            {
                enums.Add((TEnum)item);
            }
            var objectedEnums = enums.Select(x => new { id = x, title = Enum.GetName(typeof(TEnum), x) });

            return Json(objectedEnums, BOT_SERIALIZER_SETTINGS);
        }
        [HttpGet]
        // GET: api/Entities/
        public virtual IHttpActionResult Get(HttpRequestMessage request, int id)
        {
            List<TEnum> enums = new List<TEnum>();
            foreach (var item in Enum.GetValues(typeof(TEnum)))
            {
                enums.Add((TEnum)item);
            }
            var objectedEnums = enums.Select(x => new { id = x, title = Enum.GetName(typeof(TEnum), x) });
            var foundedEnum = objectedEnums.FirstOrDefault(y=>Convert.ToInt32(y.id)==id);
            if(foundedEnum==null)
            {
                return BadRequest();
            }
            return Json(foundedEnum, BOT_SERIALIZER_SETTINGS);
        }
    }
}
