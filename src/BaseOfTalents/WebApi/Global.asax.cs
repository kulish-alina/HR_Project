using System.Web.Http;

namespace WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public object Assemby { get; private set; }
        public object DependencyResolver { get; private set; }

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AutofacWebApiConfiguration.Initialize(GlobalConfiguration.Configuration);
            AutoMapperWebConfiguration.Configure();
        }
    }
}
