using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using DAL;
using DAL.DTO;
using DAL.DTO.SetupDTO;
using DAL.Infrastructure;
using DAL.Repositories;
using DAL.Services;
using Domain.Entities.Enum;
using Domain.Entities.Enum.Setup;
using Entities.Enum;
using WebUI.Infrastructure.Auth;
using WebUI.Services;
using WebUI.Services.Auth;

namespace WebUI.App_Start
{
    public class AutofacConfig
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

            builder.RegisterType<BOTContext>()
                .As<System.Data.Entity.DbContext>()
                .InstancePerRequest();

            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerRequest();

            builder.RegisterType<AccessRightService>()
                .As<BaseEnumService<AccessRight>>()
                .InstancePerRequest();

            builder.RegisterType<CommentTypeService>()
                .As<BaseEnumService<CommentType>>()
                .InstancePerRequest();

            builder.RegisterType<CountryService>()
                .As<BaseService<Country, CountryDTO>>()
                .InstancePerRequest();

            builder.RegisterType<DepartmentGroupService>()
                .As<BaseService<DepartmentGroup, DepartmentGroupDTO>>()
                .InstancePerRequest();

            builder.RegisterType<DepartmentService>()
                .As<BaseService<Department, DepartmentDTO>>()
                .InstancePerRequest();

            builder.RegisterType<EntityStateService>()
                .As<BaseEnumService<EntityState>>()
                .InstancePerRequest();

            builder.RegisterType<EventTypeService>()
                .As<BaseService<EventType, EventTypeDTO>>()
                .InstancePerRequest();

            builder.RegisterType<IndustryService>()
                .As<BaseService<Industry, IndustryDTO>>()
                .InstancePerRequest();

            builder.RegisterType<LanguageLevelService>()
                .As<BaseEnumService<LanguageLevel>>()
                .InstancePerRequest();

            builder.RegisterType<LanguageService>()
                .As<BaseService<Language, LanguageDTO>>()
                .InstancePerRequest();

            builder.RegisterType<LevelService>()
                .As<BaseService<Level, LevelDTO>>()
                .InstancePerRequest();

            builder.RegisterType<CurrencyService>()
                .As<BaseService<Currency, CurrencyDTO>>()
                .InstancePerRequest();

            builder.RegisterType<CityService>()
                .As<BaseService<City, CityDTO>>()
                .InstancePerRequest();

            builder.RegisterType<PermissionService>()
                .As<BaseService<Permission, PermissionDTO>>()
                .InstancePerRequest();

            builder.RegisterType<RoleService>()
                .As<BaseService<Role, RoleDTO>>()
                .InstancePerRequest();

            builder.RegisterType<SkillService>()
                .As<BaseService<Skill, SkillDTO>>()
                .InstancePerRequest();

            builder.RegisterType<SocialNetworkService>()
                .As<BaseService<SocialNetwork, SocialNetworkDTO>>()
                .InstancePerRequest();

            builder.RegisterType<ParsingSourceService>()
                .As<BaseEnumService<ParsingSource>>()
                .InstancePerRequest();

            builder.RegisterType<StageService>()
                .As<BaseService<Stage, StageDTO>>()
                .InstancePerRequest();

            builder.RegisterType<SourceService>()
                .As<SourceService>()
                .InstancePerRequest();

            builder.RegisterType<TagService>()
                .As<BaseService<Tag, TagDTO>>()
                .InstancePerRequest();

            builder.RegisterType<TypeOfEmploymentService>()
                .As<BaseEnumService<TypeOfEmployment>>()
                .InstancePerRequest();

            builder.RegisterType<FileService>()
                .As<FileService>()
                .InstancePerRequest();

            builder.RegisterType<CandidateService>()
                .As<CandidateService>()
                .InstancePerRequest();

            builder.RegisterType<UserService>()
                .As<UserService>()
                .InstancePerRequest();

            builder.RegisterType<RoleService>()
                .As<RoleService>()
                .InstancePerRequest();

            builder.RegisterType<EventService>()
                .As<EventService>()
                .InstancePerRequest();

            builder.RegisterType<VacancyService>()
                .As<VacancyService>()
                .InstancePerRequest();

            builder.RegisterType<NoteService>()
                .As<NoteService>()
                .InstancePerRequest();

            builder.RegisterGeneric(typeof(BaseRepository<>))
                .As(typeof(IRepository<>))
                .InstancePerRequest();

            builder.RegisterType<TokenContainer>()
                .As<IAuthContainer<string>>()
                .SingleInstance();

            builder.RegisterType<UserAccountService>()
                .As<IAccountService>()
                .InstancePerRequest();

            Container = builder.Build();
            return Container;

        }
    }
}