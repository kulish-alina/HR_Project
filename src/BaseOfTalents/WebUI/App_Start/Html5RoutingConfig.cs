using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;

namespace WebUI.App_Start
{
    using AppFunc = Func<IDictionary<string, object>, Task>;
    public static class Html5RoutingConfig
    {
        public static IAppBuilder UseHtml5Routing(this IAppBuilder builder, string rootPath, string entryPath)
        {
            var fileSystem = new PhysicalFileSystem(rootPath);
            var fileServerOptions = new FileServerOptions()
            {
                EnableDirectoryBrowsing = false,
                FileSystem = fileSystem
            };
            var options = new HTML5ServerOptions()
            {
                FileServerOptions = fileServerOptions,
                EntryPath = new PathString(entryPath)
            };

            builder.UseDefaultFiles(options.FileServerOptions.DefaultFilesOptions);

            return builder.Use(new Func<AppFunc, AppFunc>(next => new Html5Middleware(next, options).Invoke));
        }
    }

    public class HTML5ServerOptions
    {
        public FileServerOptions FileServerOptions { get; set; }

        public PathString EntryPath { get; set; }

        public bool Html5Mode
        {
            get
            {
                return EntryPath.HasValue;
            }
        }

        public HTML5ServerOptions()
        {
            FileServerOptions = new FileServerOptions();
            EntryPath = PathString.Empty;
        }
    }

    public class Html5Middleware
    {
        private readonly HTML5ServerOptions _options;
        private readonly AppFunc _next;
        private readonly StaticFileMiddleware _innerMiddleware;

        public Html5Middleware(AppFunc next, HTML5ServerOptions options)
        {
            _next = next;
            _options = options;

            _innerMiddleware = new StaticFileMiddleware(next, options.FileServerOptions.StaticFileOptions);
        }

        public async Task Invoke(IDictionary<string, object> arg)
        {
            await _innerMiddleware.Invoke(arg);

            if ((int)arg["owin.ResponseStatusCode"] == 404
                && _options.Html5Mode)
            {
                arg["owin.RequestPath"] = _options.EntryPath.Value;
                await _innerMiddleware.Invoke(arg);
            }
        }
    }
}