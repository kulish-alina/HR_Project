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
using WebUI.Auth.Infrastructure;
using WebUI.Auth.Services;
using WebUI.Services;
using WebUI.Helpers;

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
                .InstancePerDependency();

            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerDependency();

            builder.RegisterType<AccessRightService>()
                .As<BaseEnumService<AccessRight>>()
                .InstancePerDependency();

            builder.RegisterType<CommentTypeService>()
                .As<BaseEnumService<CommentType>>()
                .InstancePerDependency();

            builder.RegisterType<CountryService>()
                .As<BaseService<Country, CountryDTO>>()
                .InstancePerDependency();

            builder.RegisterType<MailService>()
                .As<BaseService<MailContent, MailDTO>>()
                .InstancePerDependency();

            builder.RegisterType<TemplateLoader>()
                .As<ITemplateLoader>()
                .InstancePerDependency();

            builder.RegisterType<TemplateService>()
                .As<TemplateService>()
                .InstancePerDependency();

            builder.RegisterType<CVParserService>()
                .As<CVParserService>()
                .InstancePerDependency();

            builder.RegisterType<DepartmentGroupService>()
                .As<BaseService<DepartmentGroup, DepartmentGroupDTO>>()
                .InstancePerDependency();

            builder.RegisterType<DepartmentService>()
                .As<BaseService<Department, DepartmentDTO>>()
                .InstancePerDependency();

            builder.RegisterType<EntityStateService>()
                .As<BaseEnumService<EntityState>>()
                .InstancePerDependency();

            builder.RegisterType<EventTypeService>()
                .As<BaseService<EventType, EventTypeDTO>>()
                .InstancePerDependency();

            builder.RegisterType<IndustryService>()
                .As<BaseService<Industry, IndustryDTO>>()
                .InstancePerDependency();

            builder.RegisterType<LanguageLevelService>()
                .As<BaseEnumService<LanguageLevel>>()
                .InstancePerDependency();

            builder.RegisterType<LanguageService>()
                .As<BaseService<Language, LanguageDTO>>()
                .InstancePerDependency();

            builder.RegisterType<LevelService>()
                .As<BaseService<Level, LevelDTO>>()
                .InstancePerDependency();

            builder.RegisterType<CurrencyService>()
                .As<BaseService<Currency, CurrencyDTO>>()
                .InstancePerDependency();

            builder.RegisterType<CityService>()
                .As<BaseService<City, CityDTO>>()
                .InstancePerDependency();

            builder.RegisterType<PermissionService>()
                .As<BaseService<Permission, PermissionDTO>>()
                .InstancePerDependency();

            builder.RegisterType<RoleService>()
                .As<BaseService<Role, RoleDTO>>()
                .InstancePerDependency();

            builder.RegisterType<SkillService>()
                .As<BaseService<Skill, SkillDTO>>()
                .InstancePerDependency();

            builder.RegisterType<SocialNetworkService>()
                .As<BaseService<SocialNetwork, SocialNetworkDTO>>()
                .InstancePerDependency();

            builder.RegisterType<ParsingSourceService>()
                .As<BaseEnumService<ParsingSource>>()
                .InstancePerDependency();

            builder.RegisterType<StageService>()
                .As<BaseService<Stage, StageDTO>>()
                .InstancePerDependency();

            builder.RegisterType<SourceService>()
                .As<SourceService>()
                .InstancePerDependency();

            builder.RegisterType<TagService>()
                .As<BaseService<Tag, TagDTO>>()
                .InstancePerDependency();

            builder.RegisterType<TypeOfEmploymentService>()
                .As<BaseEnumService<TypeOfEmployment>>()
                .InstancePerDependency();

            builder.RegisterType<FileService>()
                .As<FileService>()
                .InstancePerDependency();

            builder.RegisterType<CandidateService>()
                .As<CandidateService>()
                .InstancePerDependency();

            builder.RegisterType<UserService>()
                .As<UserService>()
                .InstancePerDependency();

            builder.RegisterType<RoleService>()
                .As<RoleService>()
                .InstancePerDependency();

            builder.RegisterType<ReportService>()
                .As<ReportService>()
                .InstancePerDependency();

            builder.RegisterType<EventService>()
                .As<EventService>()
                .InstancePerDependency();

            builder.RegisterType<VacancyService>()
                .As<VacancyService>()
                .InstancePerDependency();

            builder.RegisterType<NoteService>()
                .As<NoteService>()
                .InstancePerDependency();

            builder.RegisterGeneric(typeof(BaseRepository<>))
                .As(typeof(IRepository<>))
                .InstancePerDependency();

            builder.RegisterType<UserAccountService>()
                .As<IAccountService>()
                .InstancePerDependency();

            builder.RegisterType<ExportService>()
                .As<ExportService>()
                .InstancePerDependency();

            builder.RegisterType<ExportConverter>()
                .As<IConverter<Dictionary<int, List<CandidateProgressReportUnitDTO>>, ExportDataSet>>()
                .InstancePerDependency();

            builder.RegisterType<ExcelExporter>()
                .As<IExporter<ExportDataSet>>()
                .InstancePerDependency();

            Container = builder.Build();
            return Container;

        }
    }
}