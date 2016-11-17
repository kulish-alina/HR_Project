using System.Collections.Generic;
using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using DAL;
using DAL.DTO;
using DAL.DTO.ReportDTO;
using DAL.DTO.SetupDTO;
using DAL.Infrastructure;
using DAL.Repositories;
using DAL.Services;
using Domain.Entities;
using Domain.Entities.Enum;
using Domain.Entities.Enum.Setup;
using Entities.Enum;
using Exporter;
using Mailer;
using WebUI.Helpers;
using WebUI.Infrastructure.Auth;
using WebUI.Services;
using WebUI.Services.Auth;

namespace WebUI.App_Start
{
    public class AutofacConfig
    {
        public static IContainer Container;
        public static IContainer Initialize(HttpConfiguration config)
        {
            return Initialize(config, RegisterServices(new ContainerBuilder()));
        }
        public static IContainer Initialize(HttpConfiguration config, IContainer container)
        {
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            return container;
        }

        private static IContainer RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<BotContextFactory>()
                .As<IContextFactory>()
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

            builder.RegisterType<MailService>()
                .As<BaseService<MailContent, MailDTO>>()
                .InstancePerRequest();

            builder.RegisterType<TemplateLoader>()
                .As<ITemplateLoader>()
                .InstancePerRequest();

            builder.RegisterType<TemplateService>()
                .As<TemplateService>()
                .InstancePerRequest();

            builder.RegisterType<CVParserService>()
                .As<CVParserService>()
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

            builder.RegisterType<ReportService>()
                .As<ReportService>()
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

            builder.RegisterType<ExportService>()
                .As<ExportService>()
                .InstancePerRequest();

            builder.RegisterType<ExportConverter>()
                .As<IConverter<Dictionary<int, List<CandidateProgressReportUnitDTO>>, ExportDataSet>>()
                .InstancePerRequest();

            builder.RegisterType<ExcelExporter>()
                .As<IExporter<ExportDataSet>>()
                .InstancePerRequest();

            Container = builder.Build();
            return Container;

        }
    }
}