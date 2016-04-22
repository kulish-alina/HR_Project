using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EFData.Design
{
    public interface IRepositoryFacade
    {
        ICandidateRepository CandidateRepository { get; }
        ILocationRepository CityRepository { get; }
        ICommentRepository CommentRepository { get; }
        IEventRepository EventRepository { get; }
        IEventTypeRepository EventTypeRepository { get; }
        IFileRepository FileRepository { get; }
        IPermissionRepository PermissionRepository { get; }
        IPhotoRepository PhotoRepository { get; }
        IRoleRepository RoleRepository { get; }
        IStageRepository StageRepository { get; }
        ICountryRepository CountryRepository { get; }
        ILanguageRepository LanguageRepository { get; }
        ISkillRepository SkillRepository { get; }
        ISocialNetworkRepository SocialNetworkRepository { get; }
        IDepartmentGroupRepository DepartmentGroupRepository { get; }
        IDepartmentRepository DepartmentRepository { get; }
        IVacancyRepository VacancyRepository { get; }
    }
}
