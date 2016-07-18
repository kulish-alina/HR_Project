using System.Web.Http;
using System.Web.Http.Cors;

namespace WebUI.App_Start
{
    public static class WebApiConfig
    {
        public static HttpConfiguration Create()
        {
            // Web API configuration and services
            var config = new HttpConfiguration();

            AutomapperConfig.Configure();
            AutofacConfig.Initialize(config);

            var corsAtts = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(corsAtts);

            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                 name: "DefaultApi",
                 routeTemplate: "api/{controller}/{id}",
                 defaults: new { id = RouteParameter.Optional }
             );

            return config;
        }
    }
}