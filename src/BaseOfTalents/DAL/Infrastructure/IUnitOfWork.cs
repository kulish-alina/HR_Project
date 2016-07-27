namespace DAL.Infrastructure
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepo { get; }
        IVacancyRepository VacancyRepo { get; }
        ICandidateRepository CandidateRepo { get; }
        IFileRepository FileRepo { get; }
        ILevelRepository LevelRepo { get; }
        ICityRepository CityRepo { get; }
        ITagRepository TagRepo { get; }
        ISkillRepository SkillRepo { get; }
        ILanguageSkillRepository LanguageSkillRepo { get; }
        ILanguageRepository LanguageRepo { get; }
        ICountryRepository CountryRepo { get; }
        IDepartmentGroupRepository DepartmentGroupRepo { get; }
        IDepartmentRepository DepartmentRepo { get; }
        IEventTypeRepository EventTypeRepo { get; }
        IIndustryRepository IndustryRepo { get; }
        IPermissionRepository PermissionRepo { get; }
        IRoleRepository RoleRepo { get; }
        ISocialNetworkRepository SocialNetworkRepo { get; }
        IStageRepository StageRepo { get; }
        IPhoneNumberRepository PhoneNumberRepo { get; }
        ICommentRepository CommentRepo { get; }
        ICandidateSocialRepository CandidateSocialRepo { get; }
        ICandidateSourceRepository CandidateSourceRepo { get; }
        IVacancyStageInfoRepository VacancyStageInfoRepo { get; }
        IEventRepository EventRepo { get; }
        INoteRepository NoteRepo { get; }
        ICurrencyRepository CurrencyRepo { get; }
        ISourceRepository SourceRepo { get; }


        void Commit();
    }
}