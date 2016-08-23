using AutoMapper;
using DAL.DTO;
using DAL.DTO.SetupDTO;
using Domain.Entities;
using Domain.Entities.Enum.Setup;
using System.Collections.Generic;
using WebUI.Globals.Converters;
using WebUI.Models;

namespace WebUI.App_Start
{
    public static class AutomapperConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(x =>
            {
                x.CreateMap<CandidateSocial, CandidateSocialDTO>();
                x.CreateMap<CandidateSocialDTO, CandidateSocial>();

                x.CreateMap<Comment, CommentDTO>();
                x.CreateMap<CommentDTO, Comment>();

                x.CreateMap<Country, CountryDTO>();
                x.CreateMap<CountryDTO, Country>();

                x.CreateMap<Department, DepartmentDTO>();
                x.CreateMap<DepartmentDTO, Department>();

                x.CreateMap<DepartmentGroup, DepartmentGroupDTO>();
                x.CreateMap<DepartmentGroupDTO, DepartmentGroup>();

                x.CreateMap<EventType, EventTypeDTO>();
                x.CreateMap<EventTypeDTO, EventType>();

                x.CreateMap<Event, EventDTO>();
                x.CreateMap<EventDTO, Event>();

                x.CreateMap<Industry, IndustryDTO>();
                x.CreateMap<IndustryDTO, Industry>();

                x.CreateMap<Language, LanguageDTO>();
                x.CreateMap<LanguageDTO, Language>();

                x.CreateMap<PhoneNumber, PhoneNumberDTO>();
                x.CreateMap<PhoneNumberDTO, PhoneNumber>();

                x.CreateMap<Level, LevelDTO>();
                x.CreateMap<LevelDTO, Level>();

                x.CreateMap<City, CityDTO>();
                x.CreateMap<CityDTO, City>();

                x.CreateMap<Skill, SkillDTO>();
                x.CreateMap<SkillDTO, Skill>();

                x.CreateMap<SocialNetwork, SocialNetworkDTO>();
                x.CreateMap<SocialNetworkDTO, SocialNetwork>();

                x.CreateMap<Stage, StageDTO>();
                x.CreateMap<StageDTO, Stage>();

                x.CreateMap<ExtendedStage, ExtendedStageDTO>()
                    .ForMember(dest => dest.Stage, opt => opt.MapFrom(src => Mapper.Map<Stage, StageDTO>(src.Stage)));
                x.CreateMap<StageDTO, Stage>();

                x.CreateMap<Source, SourceDTO>();
                x.CreateMap<SourceDTO, Source>();

                x.CreateMap<Tag, TagDTO>();
                x.CreateMap<TagDTO, Tag>();

                x.CreateMap<File, FileDTO>();
                x.CreateMap<FileDTO, File>();

                x.CreateMap<Note, NoteDTO>();
                x.CreateMap<NoteDTO, Note>();

                x.CreateMap<Currency, CurrencyDTO>();
                x.CreateMap<CurrencyDTO, Currency>();

                x.CreateMap<CandidateSource, CandidateSourceDTO>();
                x.CreateMap<CandidateSourceDTO, CandidateSource>();

                x.CreateMap<LanguageSkill, LanguageSkillDTO>();
                x.CreateMap<LanguageSkillDTO, LanguageSkill>();

                x.CreateMap<VacancyStageInfo, VacancyStageInfoDTO>()
                    .ForMember(dest => dest.StageId, opt => opt.MapFrom(src => Mapper.Map<Stage, int>(src.Stage)));
                x.CreateMap<VacancyStageInfoDTO, VacancyStageInfo>();

                x.CreateMap<Vacancy, int>()
                     .ConstructUsing(source => (source as Vacancy).Id);

                x.CreateMap<Stage, int>()
                     .ConstructUsing(source => (source as Stage).Id);

                x.CreateMap<Currency, int>()
                     .ConstructUsing(source => (source as Currency).Id);

                x.CreateMap<Vacancy, int>()
                    .ConstructUsing(source => (source as Vacancy).Id);

                x.CreateMap<Skill, int>()
                     .ConstructUsing(source => (source as Skill).Id);

                x.CreateMap<Permission, int>()
                    .ConstructUsing(source => (source as Permission).Id);

                x.CreateMap<Role, int>()
                        .ConstructUsing(source => (source as Role).Id);

                x.CreateMap<Tag, int>()
                     .ConstructUsing(source => (source as Tag).Id);

                x.CreateMap<Level, int>()
                     .ConstructUsing(source => (source as Level).Id);

                x.CreateMap<Department, int>()
                    .ConstructUsing(source => (source as Department).Id);

                x.CreateMap<City, int>()
                    .ConstructUsing(source => (source as City).Id);

                x.CreateMap<Role, RoleDTO>()
                    .ForMember(roleDto => roleDto.Permissions, opt => opt.MapFrom(src => PermissionConverter.Convert(src.Permissions)));

                x.CreateMap<Permission, PermissionDTO>()
                        .ForMember(dest => dest.RoleIds, opt => opt.MapFrom(src => Mapper.Map<IEnumerable<Role>, IEnumerable<int>>(src.Roles)));

                x.CreateMap<User, UserDTO>()
                   .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => Mapper.Map<File, FileDTO>(src.Photo)))
                   .ForMember(dest => dest.PhoneNumbers, opt => opt.MapFrom(src => Mapper.Map<IEnumerable<PhoneNumberDTO>>(src.PhoneNumbers)));

                x.CreateMap<Candidate, CandidateDTO>()
                    .ForMember(dest => dest.ClosedVacanciesIds, opt => opt.MapFrom(src => Mapper.Map<IEnumerable<Vacancy>, IEnumerable<int>>(src.ClosedVacancies)))
                    .ForMember(dest => dest.VacanciesProgress, opt => opt.MapFrom(src => Mapper.Map<IEnumerable<VacancyStageInfo>, IEnumerable<VacancyStageInfoDTO>>(src.VacanciesProgress)))
                    .ForMember(dest => dest.SocialNetworks, opt => opt.MapFrom(src => Mapper.Map<IEnumerable<CandidateSocial>, IEnumerable<CandidateSocialDTO>>(src.SocialNetworks)))
                    .ForMember(dest => dest.LanguageSkills, opt => opt.MapFrom(src => Mapper.Map<IEnumerable<LanguageSkill>, IEnumerable<LanguageSkillDTO>>(src.LanguageSkills)))
                    .ForMember(dest => dest.TagIds, opt => opt.MapFrom(src => Mapper.Map<IEnumerable<Tag>, IEnumerable<int>>(src.Tags)))
                    .ForMember(dest => dest.SkillIds, opt => opt.MapFrom(src => Mapper.Map<IEnumerable<Skill>, IEnumerable<int>>(src.Skills)))
                    .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => Mapper.Map<IEnumerable<Comment>, IEnumerable<CommentDTO>>(src.Comments)))
                    .ForMember(dest => dest.Events, opt => opt.MapFrom(src => Mapper.Map<IEnumerable<Event>, IEnumerable<EventDTO>>(src.Events)))
                    .ForMember(dest => dest.RelocationPlaces, opt => opt.MapFrom(src => Mapper.Map<IEnumerable<RelocationPlace>, IEnumerable<RelocationPlaceDTO>>(src.RelocationPlaces)))
                    .ForMember(dest => dest.Files, opt => opt.MapFrom(src => Mapper.Map<IEnumerable<File>, IEnumerable<FileDTO>>(src.Files)));

                x.CreateMap<Vacancy, VacancyDTO>()
                    .ForMember(dest => dest.CandidatesProgress, opt => opt.MapFrom(src => Mapper.Map<IEnumerable<VacancyStageInfo>, IEnumerable<VacancyStageInfoDTO>>(src.CandidatesProgress)))
                    .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => Mapper.Map<IEnumerable<Comment>, IEnumerable<CommentDTO>>(src.Comments)))
                    .ForMember(dest => dest.LevelIds, opt => opt.MapFrom(src => Mapper.Map<IEnumerable<Level>, IEnumerable<int>>(src.Levels)))
                    .ForMember(dest => dest.CityIds, opt => opt.MapFrom(src => Mapper.Map<IEnumerable<City>, IEnumerable<int>>(src.Cities)))
                    .ForMember(dest => dest.TagIds, opt => opt.MapFrom(src => Mapper.Map<IEnumerable<Tag>, IEnumerable<int>>(src.Tags)))
                    .ForMember(dest => dest.RequiredSkillIds, opt => opt.MapFrom(src => Mapper.Map<IEnumerable<Skill>, IEnumerable<int>>(src.RequiredSkills)))
                    .ForMember(dest => dest.ChildVacanciesIds, opt => opt.MapFrom(src => Mapper.Map<IEnumerable<Vacancy>, IEnumerable<int>>(src.ChildVacancies)))
                    .ForMember(dest => dest.Files, opt => opt.MapFrom(src => Mapper.Map<IEnumerable<File>, IEnumerable<FileDTO>>(src.Files)))
                    .ForMember(dest => dest.StageFlow, opt => opt.MapFrom(src => Mapper.Map<IEnumerable<ExtendedStage>, IEnumerable<ExtendedStageDTO>>(src.StageFlow)));


                x.CreateMap<RelocationPlace, RelocationPlaceDTO>();
                x.CreateMap<RelocationPlaceDTO, RelocationPlace>();

                x.CreateMap<VacancyDTO, VacancySearchModel>()
                   .ForMember(dest => dest.State, opt => opt.MapFrom(src => (int)src.State));
            });
        }
    }
}
