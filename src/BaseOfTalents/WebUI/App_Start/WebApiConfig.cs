using System.Web.Http;
using System.Web.Http.Cors;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WebUI.App_Start
{
    public static class WebApiConfig
    {
        public static HttpConfiguration Create()
        {
            return new HttpConfiguration();
        }
    }

    public static class HttpConfigurationExtensions
    {
        public static HttpConfiguration ConfigureCors(this HttpConfiguration config)
        {
            var corsAtts = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(corsAtts);
            return config;
        }

        public static HttpConfiguration ConfigureRouting(this HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                 name: "DefaultApi",
                 routeTemplate: "api/{controller}/{id}",
                 defaults: new { id = RouteParameter.Optional }
             );

            return config;
        }

        public static HttpConfiguration ConfigJsonSerialization(this HttpConfiguration config)
        {
            config.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
#if DEBUG
            config.Formatters.JsonFormatter.SerializerSettings.Formatting = Formatting.Indented;
#else
            config.Formatters.JsonFormatter.SerializerSettings.Formatting = Formatting.None;
#endif
            return config;
        }
    }
}