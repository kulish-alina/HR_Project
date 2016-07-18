using System;
using System.IO;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Microsoft.Owin.StaticFiles.ContentTypes;
using Owin;

namespace WebUI.App_Start
{
    public static class StaticFilesConfig
    {
        public static IAppBuilder UseStaticFilesServer(this IAppBuilder app, string path)
        {
            var provider = new FileExtensionContentTypeProvider();
            provider.Mappings.Add(".odt", "application/vnd.oasis.opendocument.text");

            return app.UseStaticFiles(new StaticFileOptions()
            {
                FileSystem = new PhysicalFileSystem(Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, Globals.Constants.RootFolder, Globals.Constants.Uploads)),
                //RequestPath = new PathString(path),
                ContentTypeProvider = provider
            });
        }
    }
}