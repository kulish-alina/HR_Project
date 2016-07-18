using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;
using WebUI.App_Start;

[assembly: OwinStartup(typeof(BaseOfTalents.WebUI.ApiStartup))]

namespace BaseOfTalents.WebUI
{
    public class ApiStartup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            app.UseWebApi(config);

            const string rootFolder = ".";
            var fileSystem = new PhysicalFileSystem(rootFolder);
            var options = new FileServerOptions
            {
                EnableDefaultFiles = true,
                FileSystem = fileSystem
            };

            app.UseFileServer(options);
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
        }
    }
}
