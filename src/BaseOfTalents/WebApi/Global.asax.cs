using Autofac;
using Autofac.Integration.WebApi;
using Data.EFData;
using Data.EFData.Repositories;
using Data.Infrastructure;
using Domain.Repositories;
using System.Data.Entity;
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
            AutofacWebApiConfiguration.Initialize(GlobalConfiguration.Configuration);
            AutoMapperWebConfiguration.Configure();
        }
    }
}
