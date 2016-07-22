using DAL.Infrastructure;
using DAL.Repositories;
using System.Data.Entity;

namespace DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext context;

        private IFileRepository fileRepo;
        private ICandidateRepository candidateRepo;
        private IUserRepository userRepo;
        private IVacancyRepository vacancyRepo;
        private ILevelRepository levelRepo;
        private ICityRepository cityRepo;
        private ITagRepository tagRepo;
        private ISkillRepository skillRepo;
        private ILanguageSkillRepository languageSkillRepo;
        private ILanguageRepository languageRepo;
        private ICountryRepository countryRepo;
        private IDepartmentGroupRepository departmentGroupRepo;
        private IDepartmentRepository departmentRepo;
        private IEventTypeRepository eventTypeRepo;
        private IIndustryRepository industryRepo;
        private IPermissionRepository permissionRepo;
        private IRoleRepository roleRepo;
        private ISocialNetworkRepository socialNetworkRepo;
        private IStageRepository stageRepo;
        private IPhoneNumberRepository phoneNumberRepo;
        private IVacancyStageInfoRepository vacancyStageInfoRepo;
        private ICandidateSocialRepository candidateSocialRepo;
        private ICandidateSourceRepository candidateSourceRepo;
        private ICommentRepository commentRepo;
        private IEventRepository eventRepo;
        private INoteRepository noteRepo;
        private ICurrencyRepository currencyRepo;
        private ISourceRepository sourceRepo;



        public UnitOfWork(DbContext context)
        {
            this.context = context;
        }

        public UnitOfWork()
        {
            context = new BOTContext();
        }

        public ILanguageRepository LanguageRepo
        {
            get
            {
                if (languageRepo == null)
                {
                    languageRepo = new LanguageRepository(context);
                }

                return languageRepo;
            }
        }
        public ICountryRepository CountryRepo
        {
            get
            {
                if (countryRepo == null)
                {
                    countryRepo = new CountryRepository(context);
                }

                return countryRepo;
            }
        }


        public ICandidateRepository CandidateRepo
        {
            get
            {
                if (candidateRepo == null)
                {
                    candidateRepo = new CandidateRepository(context);
                }

                return candidateRepo;
            }
        }

        public IFileRepository FileRepo
        {
            get
            {
                if (fileRepo == null)
                {
                    fileRepo = new FileRepository(context);
                }

                return fileRepo;
            }
        }

        public IUserRepository UserRepo
        {
            get
            {
                if (userRepo == null)
                {
                    userRepo = new UserRepository(context);
                }

                return userRepo;
            }
        }

        public ILevelRepository LevelRepo
        {
            get
            {
                if (levelRepo == null)
                {
                    levelRepo = new LevelRepository(context);
                }

                return levelRepo;
            }
        }

        public IVacancyRepository VacancyRepo
        {
            get
            {
                if (vacancyRepo == null)
                {
                    vacancyRepo = new VacancyRepository(context);
                }

                return vacancyRepo;
            }
        }

        public ICityRepository CityRepo
        {
            get
            {
                if (cityRepo == null)
                {
                    cityRepo = new CityRepository(context);
                }

                return cityRepo;
            }
        }

        public ITagRepository TagRepo
        {
            get
            {
                if (tagRepo == null)
                {
                    tagRepo = new TagRepository(context);
                }

                return tagRepo;
            }
        }


        public ISkillRepository SkillRepo
        {
            get
            {
                if (skillRepo == null)
                {
                    skillRepo = new SkillRepository(context);
                }

                return skillRepo;
            }
        }
        public ILanguageSkillRepository LanguageSkillRepo
        {
            get
            {
                if (languageSkillRepo == null)
                {
                    languageSkillRepo = new LanguageSkillRepository(context);
                }

                return languageSkillRepo;
            }
        }


        public IDepartmentGroupRepository DepartmentGroupRepo
        {
            get
            {
                if (departmentGroupRepo == null)
                {
                    departmentGroupRepo = new DepartmentGroupRepository(context);
                }

                return departmentGroupRepo;
            }
        }

        public IDepartmentRepository DepartmentRepo
        {
            get
            {
                if (departmentRepo == null)
                {
                    departmentRepo = new DepartmentRepository(context);
                }

                return departmentRepo;
            }
        }

        public IEventTypeRepository EventTypeRepo
        {
            get
            {
                if (eventTypeRepo == null)
                {
                    eventTypeRepo = new EventTypeRepository(context);
                }

                return eventTypeRepo;
            }
        }

        public IIndustryRepository IndustryRepo
        {
            get
            {
                if (industryRepo == null)
                {
                    industryRepo = new IndustryRepository(context);
                }

                return industryRepo;
            }
        }

        public IPermissionRepository PermissionRepo
        {
            get
            {
                if (permissionRepo == null)
                {
                    permissionRepo = new PermissionRepository(context);
                }

                return permissionRepo;
            }
        }

        public IRoleRepository RoleRepo
        {
            get
            {
                if (roleRepo == null)
                {
                    roleRepo = new RoleRepository(context);
                }

                return roleRepo;
            }
        }

        public ISocialNetworkRepository SocialNetworkRepo
        {
            get
            {
                if (socialNetworkRepo == null)
                {
                    socialNetworkRepo = new SocialNetworkRepository(context);
                }

                return socialNetworkRepo;
            }
        }

        public IStageRepository StageRepo
        {
            get
            {
                if (stageRepo == null)
                {
                    stageRepo = new StageRepository(context);
                }

                return stageRepo;
            }
        }

        public IPhoneNumberRepository PhoneNumberRepo
        {
            get
            {
                if (phoneNumberRepo == null)
                {
                    phoneNumberRepo = new PhoneNumberRepository(context);
                }

                return phoneNumberRepo;
            }
        }

        public ICandidateSocialRepository CandidateSocialRepo
        {
            get
            {
                if (candidateSocialRepo == null)
                {
                    candidateSocialRepo = new CandidateSocialRepository(context);
                }

                return candidateSocialRepo;
            }
        }

        public ICandidateSourceRepository CandidateSourceRepo
        {
            get
            {
                if (candidateSourceRepo == null)
                {
                    candidateSourceRepo = new CandidateSourceRepository(context);
                }

                return candidateSourceRepo;
            }
        }

        public IVacancyStageInfoRepository VacancyStageInfoRepo
        {
            get
            {
                if (vacancyStageInfoRepo == null)
                {
                    vacancyStageInfoRepo = new VacancyStageInfoRepository(context);
                }

                return vacancyStageInfoRepo;
            }
        }

        public ICommentRepository CommentRepo
        {
            get
            {
                if (commentRepo == null)
                {
                    commentRepo = new CommentRepository(context);
                }

                return commentRepo;
            }
        }

        public IEventRepository EventRepo
        {
            get
            {
                if (eventRepo == null)
                {
                    eventRepo = new EventRepository(context);
                }

                return eventRepo;
            }
        }

        public INoteRepository NoteRepo
        {
            get
            {
                if (noteRepo == null)
                {
                    noteRepo = new NoteRepository(context);
                }

                return noteRepo;
            }
        }

        public ICurrencyRepository CurrencyRepo
        {
            get
            {
                if (currencyRepo == null)
                {
                    currencyRepo = new CurrencyRepository(context);
                }

                return currencyRepo;
            }
        }

        public ISourceRepository SourceRepo
        {
            get
            {
                if (sourceRepo == null)
                {
                    sourceRepo = new SourceRepository(context);
                }

                return sourceRepo;
            }
        }

        public void Commit()
        {
            context.SaveChanges();
        }
    }
}