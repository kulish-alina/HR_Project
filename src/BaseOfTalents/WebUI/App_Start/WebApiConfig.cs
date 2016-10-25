using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ExceptionHandling;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WebUI.Services;

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

        public static HttpConfiguration ConfigureExceptionLogging(this HttpConfiguration config)
        {
            var configuration = new NLog.Config.LoggingConfiguration();
            var redisTarget = new NLog.Targets.RedisTarget
            {
                Db = 0,
                DataType = "list",
                Host = "127.0.0.1",
                Key = "logKey",
                Name = "redis",
                Port = 6379,
                Layout = @"${date:format=yyyyMMddHHmmss} ${uppercase:${level}} ${message} ${newline}"
            };
            var loggingRule = new NLog.Config.LoggingRule("*", NLog.LogLevel.Debug, redisTarget);
            configuration.AddTarget(redisTarget);
            configuration.LoggingRules.Add(loggingRule);
            NLog.LogManager.Configuration = configuration;

            config.Services.Add(typeof(IExceptionLogger), new NLogExceptionLogger());
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