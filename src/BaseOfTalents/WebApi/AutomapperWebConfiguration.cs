using AutoMapper;
using Data.EFData.Design;
using Domain.DTO.DTOModels;
using Domain.Entities;
using Domain.Entities.Setup;
using System.Collections.Generic;

namespace WebApi
{
    public static class AutoMapperWebConfiguration
    {
        public static void Configure(IRepositoryFacade _facade)
        {
            Mapper.Initialize(x =>
            {
                x.CreateMap<CandidateSocial, CandidateSocialDTO>()
                   .ForMember(dest => dest.SocialNetworkId, opt => opt.MapFrom(src => src.SocialNetwork.Id));
                x.CreateMap<CandidateSocialDTO, CandidateSocial>()
                    .ForMember(dest => dest.SocialNetwork, opt => opt.MapFrom(src => _facade.SocialNetworkRepository.Get(src.SocialNetworkId)));

                x.CreateMap<LanguageSkill, LanguageSkillDTO>()
                    .ForMember(dest => dest.LanguageId, opt => opt.MapFrom(src => src.Language.Id));
                x.CreateMap<LanguageSkillDTO, LanguageSkill>()
                    .ForMember(dest => dest.Language, opt => opt.MapFrom(src => _facade.LanguageRepository.Get(src.LanguageId)));

                x.CreateMap<Skill, int>()
                     .ConstructUsing(source => (source.SourceValue as Skill).Id);
                x.CreateMap<int, Skill>()
                    .ConstructUsing(src => _facade.SkillRepository.Get(src));

                x.CreateMap<Department, int>()
                    .ConstructUsing(source => (source.SourceValue as Department).Id);
                x.CreateMap<int, Department>()
                     .ConstructUsing(src => _facade.DepartmentRepository.Get(src));


                x.CreateMap<Location, int>()
                    .ConstructUsing(source => (source.SourceValue as Location).Id);
                x.CreateMap<int, Location>()
                     .ConstructUsing(src => _facade.CityRepository.Get(src));

                x.CreateMap<VacancyStage, VacancyStageDTO>()
                    .ForMember(dest => dest.StageId, opt => opt.MapFrom(src => src.Stage.Id))
                    .ForMember(dest => dest.VacancyId, opt => opt.MapFrom(src => src.Vacacny.Id));

                x.CreateMap<VacancyStageInfo, VacancyStageInfoDTO>()
                    .ForMember(dest => dest.VacancyStage, opt => opt.MapFrom(src => Mapper.Map<VacancyStageDTO>(src.VacancyStage)))
                    .ForMember(dest => dest.CandidateId, opt => opt.MapFrom(src => src.Candidate.Id));

                x.CreateMap<Candidate, CandidateDTO>()
                    .ForMember(dest => dest.VacanciesProgress, opt => opt.MapFrom(src => Mapper.Map<IEnumerable<VacancyStageInfo>, IEnumerable<VacancyStageInfoDTO>>(src.VacanciesProgress)))
                    .ForMember(dest => dest.SkillsIds, opt => opt.MapFrom(src => Mapper.Map<IEnumerable<Skill>, IEnumerable<int>>(src.Skills)))
                    .ForMember(dest => dest.LocationId, opt => opt.MapFrom(src => Mapper.Map<int, int>(src.Location.Id)))
                    .ForMember(dest => dest.SocialNetworks, opt => opt.MapFrom(src => Mapper.Map<IEnumerable<CandidateSocial>, IEnumerable<CandidateSocialDTO>>(src.SocialNetworks)))
                    .ForMember(dest => dest.LanguageSkills, opt => opt.MapFrom(src => Mapper.Map<IEnumerable<LanguageSkill>, IEnumerable<LanguageSkillDTO>>(src.LanguageSkills)));
                x.CreateMap<CandidateDTO, Candidate>()
                    .ForMember(dest => dest.Skills, opt => opt.MapFrom(src => Mapper.Map<IEnumerable<int>, IEnumerable<Skill>>(src.SkillsIds)))
                    .ForMember(dest => dest.Location, opt => opt.MapFrom(src => Mapper.Map<int, Location>(src.LocationId)))
                    .ForMember(dest => dest.SocialNetworks, opt => opt.MapFrom(src => Mapper.Map<IEnumerable<CandidateSocialDTO>, IEnumerable<CandidateSocial>>(src.SocialNetworks)))
                    .ForMember(dest => dest.LanguageSkills, opt => opt.MapFrom(src => Mapper.Map<IEnumerable<LanguageSkillDTO>, IEnumerable<LanguageSkill>>(src.LanguageSkills)));

                x.CreateMap<Vacancy, VacancyDTO>()
                    .ForMember(dest => dest.CandidatesProgress, opt => opt.Ignore())
                    .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => Mapper.Map<Department, int>(src.Department)))
                    .ForMember(dest => dest.LocationIds, opt => opt.MapFrom(src => Mapper.Map<IEnumerable<Location>, IEnumerable<int>>(src.Locations)))
                    .ForMember(dest => dest.RequiredSkillsIds, opt => opt.MapFrom(src => Mapper.Map<IEnumerable<Skill>, IEnumerable<int>>(src.RequiredSkills)))
                    .ForMember(dest => dest.LanguageSkill, opt => opt.MapFrom(src => Mapper.Map<LanguageSkill, LanguageSkillDTO>(src.LanguageSkill)));
                x.CreateMap<VacancyDTO, Vacancy>()
                    .ForMember(dest => dest.Department, opt => opt.MapFrom(src => Mapper.Map<int, Department>(src.DepartmentId)))
                    .ForMember(dest => dest.Locations, opt => opt.MapFrom(src => Mapper.Map<IEnumerable<int>, IEnumerable<Location>>(src.LocationIds)))
                    .ForMember(dest => dest.RequiredSkills, opt => opt.MapFrom(src => Mapper.Map<IEnumerable<int>, IEnumerable<Skill>>(src.RequiredSkillsIds)))
                    .ForMember(dest => dest.LanguageSkill, opt => opt.MapFrom(src => Mapper.Map<LanguageSkillDTO, LanguageSkill>(src.LanguageSkill)));
            });
        }
    }
}
