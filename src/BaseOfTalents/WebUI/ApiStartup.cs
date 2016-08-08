using Microsoft.Owin;
using Owin;
using WebUI.App_Start;
using WebUI.Extensions;
using WebUI.Globals;

[assembly: OwinStartup(typeof(WebUI.ApiStartup))]
namespace WebUI
{
    public class ApiStartup
    {
        public void Configuration(IAppBuilder app)
        {
            string rootFolder = SettingsContext.Instance.GetRootPath();
            string uploadsPath = SettingsContext.Instance.GetUploadsPath();

            string defaultPage = "/index.html";

            AppConfiguration.ConfigureAutomapper();
            AppConfiguration.ConfigureJsonConverter();
            AppConfiguration.ConfigureDatabaseInitializer();
            AppConfiguration.ConfigureDirrectory(rootFolder);
            AppConfiguration.ConfigureDirrectory(uploadsPath);

            var config = WebApiConfig
                .Create()
                .ConfigureCors()
                .ConfigureRouting()
                .ConfigJsonSerialization();

            var container = AutofacConfig.Initialize(config);

            app.UseAutofacMiddleware(container)
                .UseAutofacWebApi(config)
                .UseWebApi(config)
                .UseStaticFilesServer(uploadsPath)
                .UseHtml5Routing(rootFolder, defaultPage);
        }
    }
}
