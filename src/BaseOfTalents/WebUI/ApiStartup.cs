using System;
using Autofac;
using Mailer;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Owin;
using WebUI.App_Start;
using WebUI.Auth;
using WebUI.Auth.Infrastructure;
using WebUI.Extensions;

[assembly: OwinStartup(typeof(WebUI.ApiStartup))]
namespace WebUI
{
    public class ApiStartup
    {
        public void Configuration(IAppBuilder app)
        {
            string rootFolder = Globals.SettingsContext.Instance.GetRootPath();
            string uploadsPath = Globals.SettingsContext.Instance.GetUploadsPath();
            string requestPath = Globals.SettingsContext.Instance.RequestPath;

            string defaultPage = "/index.html";

            string relativePathToEmailTemplate = "Data/email.html";

            AppConfiguration.ConfigureAutomapper();
            AppConfiguration.ConfigureJsonConverter();
            AppConfiguration.ConfigureDatabaseInitializer();
            AppConfiguration.ConfigureDirrectory(rootFolder);
            AppConfiguration.ConfigureDirrectory(uploadsPath);
            AppConfiguration.ConfigureMailAgent(Globals.SettingsContext.Instance.Email,
                Globals.SettingsContext.Instance.Password);

            TemplateLoader.SetupFile(relativePathToEmailTemplate);

            var config = WebApiConfig
                .Create()
#if RELEASE
                .ConfigureExceptionLogging()
#endif
                .ConfigureCors()
                .ConfigureRouting()
                .ConfigJsonSerialization();

            var container = AutofacConfig.Initialize(config);

            ConfigureOAuth(app, container);
            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(config);
            app.UseWebApi(config);
            app.UseStaticFilesServer(uploadsPath, requestPath);
            app.UseHtml5Routing(rootFolder, defaultPage);
        }

        private static void ConfigureOAuth(IAppBuilder app, IContainer container)
        {
            var issuer = Globals.SettingsContext.Instance.IssuerUrl;
            var secret = Globals.SettingsContext.Instance.Secret;

            using (var scope = container.BeginLifetimeScope())
            {
                app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
                {
                    //stub till we haven't enabled ssl
                    AllowInsecureHttp = true,
                    TokenEndpointPath = new PathString("/api/account/signin"),
                    AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(60),
                    Provider = new JwtAuthServer(scope.Resolve<IAccountService>()),
                    AccessTokenFormat = new ApplicationJwt(issuer, secret)
                });
            }

            app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
            {
                AuthenticationMode = AuthenticationMode.Active,
                AllowedAudiences = new[] { "Any" },
                IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
                {
                    new SymmetricKeyIssuerSecurityTokenProvider(issuer, secret)
                }
            });            
        }
    }
}
