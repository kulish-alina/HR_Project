using AutoMapper;
using Domain.DTO.DTOModels;
using System.Collections.Generic;
using Domain.DTO.DTOModels.SetupDTO;
using BaseOfTalents.Domain.Entities;
using BaseOfTalents.Domain.Entities.Enum.Setup;
using BaseOfTalents.WebUI.Models;

namespace WebApi
{
    public static class AutoMapperWebConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(x =>
            {
                x.CreateMap<CandidateSocial, CandidateSocialDTO>();
                x.CreateMap<CandidateSocialDTO, CandidateSocial>();

                x.CreateMap<Comment, CommentDTO>();
                x.CreateMap<CommentDTO, Comment>();
                
                x.CreateMap<Photo, PhotoDTO>();
                x.CreateMap<PhotoDTO, Photo>();

                x.CreateMap<Country, CountryDTO>();
                x.CreateMap<CountryDTO, Country>();

                x.CreateMap<Department, DepartmentDTO>();
                x.CreateMap<DepartmentDTO, Department>();

                x.CreateMap<DepartmentGroup, DepartmentGroupDTO>();
                x.CreateMap<DepartmentGroupDTO, DepartmentGroup>();

                x.CreateMap<EventType, EventTypeDTO>();
                x.CreateMap<EventTypeDTO, EventType>();

                x.CreateMap<Industry, IndustryDTO>();
                x.CreateMap<IndustryDTO, Industry>();

                x.CreateMap<Language, LanguageDTO>();
                x.CreateMap<LanguageDTO, Language>();

                x.CreateMap<PhoneNumber, PhoneNumberDTO>();
                x.CreateMap<PhoneNumberDTO, PhoneNumber>();

                x.CreateMap<Level, LevelDTO>();
                x.CreateMap<LevelDTO, Level>();

                x.CreateMap<Location, LocationDTO>();
                x.CreateMap<LocationDTO, Location>();

                x.CreateMap<Skill, SkillDTO>();
                x.CreateMap<SkillDTO, Skill>();

                x.CreateMap<SocialNetwork, SocialNetworkDTO>();
                x.CreateMap<SocialNetworkDTO, SocialNetwork>();

                x.CreateMap<Stage, StageDTO>();
                x.CreateMap<StageDTO, Stage>();

                x.CreateMap<Tag, TagDTO>();
                x.CreateMap<TagDTO, Tag>();

                x.CreateMap<File, FileDTO>();
                x.CreateMap<FileDTO, File>();

                x.CreateMap<CandidateSource, CandidateSourceDTO>();
                x.CreateMap<CandidateSourceDTO, CandidateSource>();

                x.CreateMap<LanguageSkill, LanguageSkillDTO>();
                x.CreateMap<LanguageSkillDTO, LanguageSkill>();

                x.CreateMap<VacancyStage, VacancyStageDTO>();
                x.CreateMap<VacancyStageDTO, VacancyStage>();

                x.CreateMap<VacancyStageInfo, VacancyStageInfoDTO>();
                x.CreateMap<VacancyStageInfoDTO, VacancyStageInfo>();

                x.CreateMap<Skill, int>()
                     .ConstructUsing(   source => (source.SourceValue as Skill).Id);

                x.CreateMap<Permission, int>()
                    .ConstructUsing(        source => (source.SourceValue as Permission).Id);

                x.CreateMap<Role, int>()
                        .ConstructUsing( source => (source.SourceValue as Role).Id);

                x.CreateMap<Tag, int>()
                     .ConstructUsing(   source => (source.SourceValue as Tag).Id);

                x.CreateMap<Level, int>()
                     .ConstructUsing(   source => (source.SourceValue as Level).Id);

                x.CreateMap<Department, int>()
                    .ConstructUsing(    source => (source.SourceValue as Department).Id);

                x.CreateMap<Location, int>()
                    .ConstructUsing(    source => (source.SourceValue as Location).Id);

                x.CreateMap<Role, RoleDTO>()
                        .ForMember(dest => dest.PermissionIds, opt => opt.MapFrom(src => Mapper.Map<IEnumerable<Permission>, IEnumerable<int>>(src.Permissions)));

                x.CreateMap<Permission, PermissionDTO>()
                        .ForMember(dest => dest.RoleIds, opt => opt.MapFrom(src => Mapper.Map<IEnumerable<Role>, IEnumerable<int>>(src.Roles)));

                x.CreateMap<User, UserDTO>()
                   .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => Mapper.Map<PhotoDTO>(src.Photo)))
                   .ForMember(dest => dest.PhoneNumbers, opt => opt.MapFrom(src => Mapper.Map<IEnumerable<PhoneNumberDTO>>(src.PhoneNumbers)));

                x.CreateMap<Candidate, CandidateDTO>()
                    .ForMember(dest => dest.VacanciesProgress,      opt => opt.MapFrom(src => Mapper.Map<IEnumerable<VacancyStageInfo>, IEnumerable<VacancyStageInfoDTO>>(src.VacanciesProgress)))
                    .ForMember(dest => dest.SocialNetworks,         opt => opt.MapFrom(src => Mapper.Map<IEnumerable<CandidateSocial>, IEnumerable<CandidateSocialDTO>>(src.SocialNetworks)))
                    .ForMember(dest => dest.LanguageSkills,         opt => opt.MapFrom(src => Mapper.Map<IEnumerable<LanguageSkill>, IEnumerable<LanguageSkillDTO>>(src.LanguageSkills)))
                    .ForMember(dest => dest.TagIds,                 opt => opt.MapFrom(src => Mapper.Map<IEnumerable<Tag>, IEnumerable<int>>(src.Tags)))
                    .ForMember(dest => dest.SkillIds,               opt => opt.MapFrom(src => Mapper.Map<IEnumerable<Skill>, IEnumerable<int>>(src.Skills)))
                    .ForMember(dest => dest.Comments,               opt => opt.MapFrom(src => Mapper.Map<IEnumerable<Comment>, IEnumerable<CommentDTO>>(src.Comments)))
                    .ForMember(dest => dest.Files,                  opt => opt.MapFrom(src => Mapper.Map<IEnumerable<File>, IEnumerable<FileDTO>>(src.Files)));

                x.CreateMap<Vacancy, VacancyDTO>()
                    .ForMember(dest => dest.CandidatesProgress,     opt => opt.MapFrom(src => Mapper.Map<IEnumerable<VacancyStageInfo>, IEnumerable<VacancyStageInfoDTO>>(src.CandidatesProgress)))
                    .ForMember(dest => dest.Comments,               opt => opt.MapFrom(src => Mapper.Map<IEnumerable<Comment>, IEnumerable<CommentDTO>>(src.Comments)))
                    .ForMember(dest => dest.LevelIds,               opt => opt.MapFrom(src => Mapper.Map<IEnumerable<Level>, IEnumerable<int>>(src.Levels)))
                    .ForMember(dest => dest.LocationIds,            opt => opt.MapFrom(src => Mapper.Map<IEnumerable<Location>, IEnumerable<int>>(src.Locations)))
                    .ForMember(dest => dest.TagIds,                 opt => opt.MapFrom(src => Mapper.Map<IEnumerable<Tag>, IEnumerable<int>>(src.Tags)))
                    .ForMember(dest => dest.RequiredSkillIds,       opt => opt.MapFrom(src => Mapper.Map<IEnumerable<Skill>, IEnumerable<int>>(src.RequiredSkills)))
                    .ForMember(dest => dest.Files,                  opt => opt.MapFrom(src => Mapper.Map<IEnumerable<File>, IEnumerable<FileDTO>>(src.Files)));

                x.CreateMap<VacancyDTO, VacancySearchModel>()
                   .ForMember(dest => dest.State, opt => opt.MapFrom(src => (int)src.State));
            });
        }
    }
}
