using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WebUI.App_Start
{
    public class JsonConverterConfig
    {
        public static void Configure()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }
    }
}