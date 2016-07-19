using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;

namespace WebUI.App_Start
{
    public static class FileSystemConfig
    {
        public static IAppBuilder UseFyleSystem(this IAppBuilder app)
        {
            const string rootFolder = "./wwwroot";
            var fileSystem = new PhysicalFileSystem(rootFolder);
            var options = new FileServerOptions
            {
                EnableDefaultFiles = true,
                FileSystem = fileSystem
            };

            return app.UseFileServer(options);
        }
    }
}