using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class BoTController : ApiController
    {
        public JsonSerializerSettings BotJsonSerializerSettings { get; set; }

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