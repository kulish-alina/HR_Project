using Autofac;
using Autofac.Integration.WebApi;
using Data.EFData;
using Data.EFData.Repositories;
using Data.Infrastructure;
using Domain.Repositories;
using System.Data.Entity;
using System.Reflection;
using System.Web.Http;
using Domain.Entities;
using Service.Services;
using Domain.DTO.DTOModels;
using Domain.Entities.Enum;		
using Domain.Entities.Enum.Setup;		
using Domain.DTO.DTOModels.SetupDTO;		
using Domain.Entities.Setup;

namespace WebApi
{
    public class AutofacWebApiConfiguration
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

            builder.RegisterType<EFErrorRepository>()
                .As<IErrorRepository>()
                .InstancePerRequest();

            builder.RegisterType<BOTContext>()
                .As<DbContext>()
                .InstancePerRequest();

            builder.RegisterType<EFCandidateRepository>()
                .As<IRepository<Candidate>>()
                .InstancePerRequest();

            builder.RegisterType<EFVacancyStageInfoRepository>()
                .As<IRepository<VacancyStageInfo>>()
                .InstancePerRequest();

            builder.RegisterType<EFVacancyRepository>()
                .As<IRepository<Vacancy>>()
                .InstancePerRequest();

            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerRequest();

            builder.RegisterType<VacancyService>()
                .As<IControllerService<Vacancy, VacancyDTO>>()
                .InstancePerRequest();

            builder.RegisterType<CandidateService>()
                .As<IControllerService<Candidate, CandidateDTO>>()
                .InstancePerRequest();

            builder.RegisterType<UserService>()
                .As<IControllerService<User, UserDTO>>()
                .InstancePerRequest();

            builder.RegisterType<FileService>()
               .As<FileService>()
               .InstancePerRequest();

            builder.RegisterGeneric(typeof(EFBaseEntityRepository<>))
            .As(typeof(IRepository<>))
            .InstancePerRequest();

            Container = builder.Build();
            return Container;

        }
    }
}
