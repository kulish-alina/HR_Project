using Mailer;
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
            string requestPath = SettingsContext.Instance.RequestPath;

            string defaultPage = "/index.html";

            AppConfiguration.ConfigureAutomapper();
            AppConfiguration.ConfigureJsonConverter();
            AppConfiguration.ConfigureDatabaseInitializer();
            AppConfiguration.ConfigureDirrectory(rootFolder);
            AppConfiguration.ConfigureDirrectory(uploadsPath);
            AppConfiguration.ConfigureMailAgent(SettingsContext.Instance.Email,
                SettingsContext.Instance.Password);

            TemplateLoader.SetupFile("Data/email.html");

            var config = WebApiConfig
                .Create()
#if RELEASE
                .ConfigureExceptionLogging()
#endif
                .ConfigureCors()
                .ConfigureRouting()
                .ConfigJsonSerialization();

            var container = AutofacConfig.Initialize(config);

            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(config);
            app.UseWebApi(config);
            app.UseStaticFilesServer(uploadsPath, requestPath);
            app.UseHtml5Routing(rootFolder, defaultPage);
        }
    }
}
