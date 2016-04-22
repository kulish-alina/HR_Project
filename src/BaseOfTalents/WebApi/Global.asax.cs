using Autofac;
using Autofac.Integration.WebApi;
using Data.EFData.Design;
using Data.EFData.Repositories;
using Domain.Repositories;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;

namespace WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public object Assemby { get; private set; }
        public object DependencyResolver { get; private set; }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            var builder = new ContainerBuilder();
            builder.RegisterType<EFRepositoryFacade>().As<IRepositoryFacade>();


            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            var container = builder.Build();
            var config = GlobalConfiguration.Configuration;

            AutoMapperWebConfiguration.Configure(container.Resolve<IRepositoryFacade>());

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}
