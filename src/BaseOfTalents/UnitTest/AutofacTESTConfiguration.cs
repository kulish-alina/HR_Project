using Autofac;
using Autofac.Integration.WebApi;
using Domain.Entities;
using Domain.Repositories;
using System.Reflection;
using System.Web.Http;
using Service.Services;
using UnitTest.DummyRepositories;

namespace UnitTest
{
    public class AutofacTESTConfiguration
    {
        public static IContainer Container;
        public static void Initialize(HttpConfiguration config)
        {
            Initialize(config, RegisterServices(new ContainerBuilder()));
        }
        public static void Initialize(HttpConfiguration config, IContainer container)
        {
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static IContainer RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            //builder.RegisterType<EFErrorRepository>()
            //    .As<IErrorRepository>()
            //    .InstancePerRequest();

            /* builder.RegisterType<BOTContext>()
                 .As<DbContext>()
            .InstancePerRequest();*/
            /*    builder.RegisterType<UnitOfWork>()
            .As<IUnitOfWork>()
            .InstancePerRequest();*/
            /*builder.RegisterType<DummyCandidateRepository>()
                .As<IRepository<Candidate>>()
                .InstancePerRequest();

            builder.RegisterType<DummyVacancyStageInfoRepository>()
                .As<IRepository<VacancyStageInfo>>()
                .InstancePerRequest();*/

            builder.RegisterType<DummyVacancyRepository>()
                .As<IRepository<Vacancy>>()
                .InstancePerRequest();


            builder.RegisterGeneric(typeof(DummyRepository<>))
            .As(typeof(IRepository<>))
            .InstancePerRequest();

            builder.RegisterGeneric(typeof(ControllerService<,>))
            .As(typeof(IControllerService<,>))
            .InstancePerRequest();

            Container = builder.Build();
            return Container;

        }
    }
}
