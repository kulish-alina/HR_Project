using Service.Extentions;
using Service.Services;
using Domain.DTO.DTOModels;
using Domain.Entities;
using Domain.Entities.Enum.Setup;
using Domain.Entities.Setup;
using Domain.Repositories;
using System;

namespace Service.Services
{
    public class CandidateService : ControllerService<Candidate, CandidateDTO>
    {
        IRepository<CandidateSocial> candidateSocialRepository;
        IRepository<CandidateSource> candidateSourceRepository;
        IRepository<PhoneNumber> phoneNumberRepository;
        IRepository<Photo> photoRepository;
        IRepository<Vacancy> vacancyRepository;
        IRepository<Skill> skillRepository;
        IRepository<Tag> tagRepository;
        IRepository<LanguageSkill> languageSkillRepository;
        IRepository<VacancyStageInfo> vacancyStageInfoRepository;
        IRepository<File> fileRepository;

        public CandidateService(
            IRepository<Candidate> candidateRepository,
            IRepository<CandidateSocial> candidateSocialRepository,
            IRepository<CandidateSource> candidateSourceRepository,
            IRepository<PhoneNumber> phoneNumberRepository,
            IRepository<Photo> photoRepository,
            IRepository<Vacancy> vacancyRepository,
            IRepository<Skill> skillRepository,
            IRepository<Tag> tagRepository,
            IRepository<LanguageSkill> languageSkillRepository,
            IRepository<VacancyStageInfo> vacancyStageInfoRepository,
            IRepository<File> fileRepository) : base(candidateRepository)
        {
            this.candidateSocialRepository = candidateSocialRepository;
            this.candidateSourceRepository = candidateSourceRepository;
            this.phoneNumberRepository = phoneNumberRepository;
            this.photoRepository = photoRepository;
            this.vacancyRepository = vacancyRepository;
            this.skillRepository = skillRepository;
            this.tagRepository = tagRepository;
            this.languageSkillRepository = languageSkillRepository;
            this.vacancyStageInfoRepository = vacancyStageInfoRepository;
            this.fileRepository = fileRepository;
        }
        public override CandidateDTO Add(CandidateDTO candidateToAdd)
        {
            Candidate _candidate = new Candidate();

            _candidate.Update(candidateToAdd, 
                skillRepository, 
                tagRepository, 
                candidateSocialRepository, 
                languageSkillRepository, 
                candidateSourceRepository,
                vacancyStageInfoRepository, 
                phoneNumberRepository,
                photoRepository, 
                vacancyRepository, 
                fileRepository);

            entityRepository.Add(_candidate);
            entityRepository.Commit();

            return DTOService.ToDTO<Candidate, CandidateDTO>(_candidate);
        }
        public override CandidateDTO Put(CandidateDTO entity)
        {
            Candidate _candidate = entityRepository.Get(entity.Id);

            _candidate.Update(entity,
                skillRepository,
                tagRepository,
                candidateSocialRepository,
                languageSkillRepository,
                candidateSourceRepository,
                vacancyStageInfoRepository,
                phoneNumberRepository,
                photoRepository,
                vacancyRepository,
                fileRepository);

            entityRepository.Update(_candidate);
            entityRepository.Commit();

            return DTOService.ToDTO<Candidate, CandidateDTO>(_candidate);
        }

        public override object Search(object searchParams)
        {
            throw new NotImplementedException();
        }
    }
}
