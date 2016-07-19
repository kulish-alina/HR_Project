using Microsoft.Owin;
using Owin;
using WebUI.App_Start;

[assembly: OwinStartup(typeof(WebUI.ApiStartup))]

namespace WebUI
{
    public class ApiStartup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = WebApiConfig.Create();

            app.UseWebApi(config);
            app.UseFyleSystem();
        }
    }
}
