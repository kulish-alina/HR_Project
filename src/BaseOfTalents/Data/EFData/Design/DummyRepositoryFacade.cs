using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Repositories;
using Data.DumbData.Repositories;

namespace Data.EFData.Design
{
    public class DummyRepositoryFacade : IRepositoryFacade
    {
        ICandidateRepository _candidateRepository;
        ILocationRepository _cityRepository;
        ICommentRepository _commentRepository;
        IEventRepository _eventRepository;
        IEventTypeRepository _eventTypeRepository;
        IFileRepository _fileRepository;
        IPermissionRepository _permissionRepository;
        IPhotoRepository _photoRepository;
        IRoleRepository _roleRepository;
        IStageRepository _stageRepository;
        ICountryRepository _countryRepository;
        ILanguageRepository _languageRepository;
        ISkillRepository _skillRepository;
        ISocialNetworkRepository _socialNetworkRepository;
        IDepartmentRepository _departmentRepository;
        IVacancyRepository _vacancyRepository;
        IDepartmentGroupRepository _departmentGroupRepository;

        public ICandidateRepository CandidateRepository
        {
            get
            {
                if (_candidateRepository == null)
                    _candidateRepository = new DummyCandidateRepository(new DumbData.DummyBotContext());
                return _candidateRepository;
            }
        }

        public ILocationRepository CityRepository
        {
            get
            {
                if (_cityRepository == null)
                    _cityRepository = new DummyCityRepository(new DumbData.DummyBotContext());
                return _cityRepository;
            }
        }

        public ICommentRepository CommentRepository
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ICountryRepository CountryRepository
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IDepartmentGroupRepository DepartmentGroupRepository
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IDepartmentRepository DepartmentRepository
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IEventRepository EventRepository
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IEventTypeRepository EventTypeRepository
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IFileRepository FileRepository
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ILanguageRepository LanguageRepository
        {
            get
            {
                if (_languageRepository == null)
                    _languageRepository = new DummyLanguageRepository(new DumbData.DummyBotContext());
                return _languageRepository;
            }
        }

        public IPermissionRepository PermissionRepository
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IPhotoRepository PhotoRepository
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IRoleRepository RoleRepository
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ISkillRepository SkillRepository
        {
            get
            {
                if (_skillRepository == null)
                    _skillRepository = new DummySkillRepository(new DumbData.DummyBotContext());
                return _skillRepository;
            }
        }

        public ISocialNetworkRepository SocialNetworkRepository
        {
            get
            {
                if (_socialNetworkRepository == null)
                    _socialNetworkRepository = new DummySocialNetworkRepository(new DumbData.DummyBotContext());
                return _socialNetworkRepository;
            }
        }

        public IStageRepository StageRepository
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IVacancyRepository VacancyRepository
        {
            get
            {
                if (_vacancyRepository == null)
                    _vacancyRepository = new DummyVacancyRepository(new DumbData.DummyBotContext());
                return _vacancyRepository;
            }
        }
    }
}
