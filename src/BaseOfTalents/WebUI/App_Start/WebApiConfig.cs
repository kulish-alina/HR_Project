using System.Web.Http;
using WebApi;

namespace BaseOfTalents.WebUI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var corsAtts = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(corsAtts);
            AutoMapperWebConfiguration.Configure();
            AutofacWebApiConfiguration.Initialize(config);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                 name: "DefaultApi",
                 routeTemplate: "api/{controller}/{id}",
                 defaults: new { id = RouteParameter.Optional }
             );
        }
    }
}