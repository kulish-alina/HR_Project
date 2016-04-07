using Autofac;
using Autofac.Integration.WebApi;
using Data.EFData.Repositories;
using Domain.Repositories;
using System.Reflection;
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
            var builder = new ContainerBuilder();

            builder.RegisterType<EFVacancyRepository>().As<IVacancyRepository>();
            builder.RegisterType<EFCandidateRepository>().As<ICandidateRepository>();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            var container = builder.Build();
            var config = GlobalConfiguration.Configuration;

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}
