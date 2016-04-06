using Autofac;
using Autofac.Integration.WebApi;
using BotData.Abstract;
using BotData.DumbData;
using BotData.DumbData.Repositories;
using BotLibrary.Repositories;
using System.Reflection;
using System.Web.Http;

namespace BotWebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public object Assemby { get; private set; }
        public object DependencyResolver { get; private set; }

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            var builder = new ContainerBuilder();

            builder.RegisterType<DummyBotContext>().As<IContext>();
            builder.RegisterType<VacancyRepository>().As<IVacancyRepository>();
            builder.RegisterType<CandidateRepository>().As<ICandidateRepository>();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            var container = builder.Build();
            var config = GlobalConfiguration.Configuration;

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}
