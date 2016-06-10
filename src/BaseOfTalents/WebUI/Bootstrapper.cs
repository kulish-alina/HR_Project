using Autofac;
using System.Web.Http;

namespace BaseOfTalents.WebUI
{
    internal static class Bootstrapper
    {
        internal static void Run()
        {
            SetAutofacWebAPI();
        }

        private static void SetAutofacWebAPI()
        {
            var configuration = GlobalConfiguration.Configuration;
            var builder = new ContainerBuilder();
            //builder.ConfigureWebApi(configuration);
        }
    }
}