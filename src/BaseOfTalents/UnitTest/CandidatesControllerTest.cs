using Data.DumbData;
using Data.DumbData.Repositories;
using Domain.Entities;
using Domain.Entities.Setup;
using NUnit.Framework;
using WebApi.Controllers;
using System.Web.Http.Results;
using Domain.Entities.Enum;
using System.Collections.Generic;
using System;
using Newtonsoft.Json.Linq;
using WebApi.DTO.DTOService;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Linq;
using Domain.Repositories;
using AutoMapper;
using Domain.DTO.DTOModels;
using Data.EFData.Design;
using System.Web.Http;

namespace UnitTest
{
    [TestFixture]
    class CandidatesControllerTest
    {
        [SetUp]
        public void StartTesting()
        {
            IRepositoryFacade _facade = new DummyRepositoryFacade();
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
                x.CreateMap<VacancyStageDTO, VacancyStage>()
                    .ForMember(dest => dest.Stage, opt => opt.MapFrom(src => _facade.StageRepository.Get(src.StageId)))
                    .ForMember(dest => dest.Vacacny, opt => opt.MapFrom(src => _facade.VacancyRepository.Get(src.VacancyId)));

                x.CreateMap<VacancyStageInfo, VacancyStageInfoDTO>()
                    .ForMember(dest => dest.VacancyStage, opt => opt.MapFrom(src => Mapper.Map<VacancyStageDTO>(src.VacancyStage)))
                    .ForMember(dest => dest.CandidateId, opt => opt.MapFrom(src => src.Candidate.Id));
                x.CreateMap<VacancyStageInfoDTO, VacancyStageInfo>()
                   .ForMember(dest => dest.VacancyStage, opt => opt.MapFrom(src => Mapper.Map<VacancyStage>(src.VacancyStage)))
                   .ForMember(dest => dest.Candidate, opt => opt.MapFrom(src => _facade.CandidateRepository.Get(src.CandidateId)));

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
            System.Diagnostics.Debugger.Launch();
        }



        [Test]
        [TestCase(1)]
        public void Get_Candidate(int id)
        {
            var _controller = new CandidatesController(new DummyRepositoryFacade());
            _controller.Request = new System.Net.Http.HttpRequestMessage();
            _controller.Configuration = new System.Web.Http.HttpConfiguration();

            var jsonResult = _controller.Get(id);
            var result = jsonResult as JsonResult<CandidateDTO>;
            Assert.AreEqual("TESTNAME", result.Content.FirstName); 
            Assert.AreEqual(id, result.Content.Id);
        }

        [Test]
        [TestCase(54235)]
        public async void Get_Candidate_BadId(int id)
        {
            var _controller = new CandidatesController(new DummyRepositoryFacade());
            _controller.Request = new System.Net.Http.HttpRequestMessage();
            _controller.Configuration = new System.Web.Http.HttpConfiguration();

            var result = _controller.Get(id);
            var response = await result.ExecuteAsync(new System.Threading.CancellationToken());

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }


        [Test]
        public void Add_Candidate()
        {
            var _controller = new CandidatesController(new DummyRepositoryFacade());
            _controller.Request = new System.Net.Http.HttpRequestMessage();
            _controller.Configuration = new System.Web.Http.HttpConfiguration();

            #region Candidate
            Comment candidateComment = new Comment()
            {
                Message = "msg",
            };

            
            File candidateFile = new File()
            {
                Description = "description",
                FilePath = "path",
            };

            CandidateSource candidateSource = new CandidateSource()
            {
                Path = "Path",
                Source = Source.HeadHunter,
            };

            Language language = new Language()
            {
                Id = 1,
                Title = "language"
            };

            LanguageSkill languageSkill = new LanguageSkill()
            {
                Language = language,
                LanguageLevel = LanguageLevel.Fluent,
            };

            Country country = new Country()
            {
                Title = "name"
            };

            Location city = new Location()
            {
                Id = 1,
                Country = country,
                Title = "dnepr"
            };

            Photo photo = new Photo()
            {
                Description = "descr",
                ImagePath = "path"
            };

            Skill skill = new Skill()
            {
                Id = 1,
                Title = "C#"
            };

            SocialNetwork socialNetwork = new SocialNetwork()
            {
                Id = 1,
                ImagePath = "path",
                Title = "Path"
            };

            CandidateSocial candidateSocial = new CandidateSocial()
            {
                Path = "path",
                SocialNetwork = socialNetwork,
            };

            Candidate candidate = new Candidate()
            {
                Skype = "skype",
                BirthDate = DateTime.Now,
                Comments = new List<Comment>() { candidateComment },
                Description = "descrpition",
                Education = "High",
                Email = "email",
                
                Files = new List<File>() { candidateFile },
                Sources = new List<CandidateSource>() { candidateSource },
                FirstName = "TESTNAME",
                IsMale = true,
                LanguageSkills = new List<LanguageSkill>() { languageSkill },
                LastName = "lname",
                Location = city,
                MiddleName = "mname",
                PhoneNumbers = new List<PhoneNumber>() { new PhoneNumber { Number = "+380978762352" } },
                Photo = photo,
                PositionDesired = "architecht",
                Practice = "best",
                RelocationAgreement = true,
                SalaryDesired = 10500,
                Skills = new List<Skill>() { skill },
                SocialNetworks = new List<CandidateSocial>() { candidateSocial },
                TypeOfEmployment = TypeOfEmployment.FullTime,
                VacanciesProgress = new List<VacancyStageInfo>() { }
            };
            #endregion
        

        }


        [Test]
        [TestCase(1)]
        public async void Delete_Candidate_OK(int id)
        {
            var _controller = new CandidatesController(new DummyRepositoryFacade());
            _controller.Request = new System.Net.Http.HttpRequestMessage();
            _controller.Configuration = new System.Web.Http.HttpConfiguration();

            var actionResponse = await _controller.Remove(id).ExecuteAsync(new System.Threading.CancellationToken());

            Assert.AreEqual(HttpStatusCode.OK, actionResponse.StatusCode);
        }

        [Test]
        [TestCase(1532)]
        public async void Delete_Candidate_NoContent(int id)
        {
            var _controller = new CandidatesController(new DummyRepositoryFacade());
            _controller.Request = new System.Net.Http.HttpRequestMessage();
            _controller.Configuration = new System.Web.Http.HttpConfiguration();


            var actionResponse = await _controller.Remove(id).ExecuteAsync(new System.Threading.CancellationToken());


            Assert.AreEqual(HttpStatusCode.NoContent, actionResponse.StatusCode);
        }

        [Test]
        [TestCase(1, "CHANGEDNAMETEST")]
        [TestCase(1, "DRUG")]
        public void Put_Candidate_OK(int id, string testname)
        {
             var _controller = new CandidatesController(new DummyRepositoryFacade());
             _controller.Request = new HttpRequestMessage();
             _controller.Configuration = new HttpConfiguration();

             var getResult = _controller.Get(id);
             var jsonGetResult = getResult as JsonResult<CandidateDTO>;
             var candidateDto = jsonGetResult.Content;

             candidateDto.FirstName = testname;

             var putResult = _controller.Put(id, candidateDto);
             var jsonPutResult = getResult as JsonResult<CandidateDTO>;
             var candidateAfterPut = jsonPutResult.Content;

             Assert.AreEqual(testname, candidateAfterPut.FirstName);
        }
    }
}