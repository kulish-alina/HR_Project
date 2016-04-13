using Data.DumbData;
using Data.DumbData.Repositories;
using Domain.Entities;
using Domain.Entities.Setup;
using NUnit.Framework;
using WebApi.Controllers;
using WebApi.DTO.DTOModels;
using AutoMapper;
using System.Web.Http.Results;
using Domain.Entities.Enum;
using System.Collections.Generic;
using System;
using Newtonsoft.Json.Linq;
using WebApi.DTO.DTOService;
using System.Diagnostics;
using System.Net;
using System.Net.Http;

namespace UnitTest
{
    [TestFixture]
    class CandidatesControllerTest
    {
        [SetUp]
        public void StartTesting()
        {
            Mapper.Initialize(x =>
            {
                x.CreateMap<Candidate, CandidateDTO>();
                x.CreateMap<CandidateDTO, Candidate>();
                x.CreateMap<Vacancy, VacancyDTO>();
                x.CreateMap<VacancyDTO, Vacancy>();
                x.CreateMap<SocialNetwork, SocialNetworkDTO>();
                x.CreateMap<SocialNetworkDTO, SocialNetwork>();
            });
           // System.Diagnostics.Debugger.Launch();
        }
        [Test]
        [TestCase(1)]
        public void Get_Candidate(int id)
        {
            var _controller = new CandidatesController(new DummyCandidateRepository(new DummyBotContext()));
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
            var _controller = new CandidatesController(new DummyCandidateRepository(new DummyBotContext()));
            _controller.Request = new System.Net.Http.HttpRequestMessage();
            _controller.Configuration = new System.Web.Http.HttpConfiguration();

            var result = _controller.Get(id);
            var response = await result.ExecuteAsync(new System.Threading.CancellationToken());
            
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }


        [Test]
        public void Add_Candidate()
        {
            var _controller = new CandidatesController(new DummyCandidateRepository(new DummyBotContext()));
            _controller.Request = new System.Net.Http.HttpRequestMessage();
            _controller.Configuration = new System.Web.Http.HttpConfiguration();

            #region Candidate
            Comment candidateComment = new Comment()
            {
                CommentType = CommentType.Candidate,
                Message = "msg",
                RelativeId = 0,
            };

            Experience experience = new Experience()
            {
                WorkExperience = new TimeSpan(1),
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
                Title = "language"
            };

            LanguageSkill languageSkill = new LanguageSkill()
            {
                Language = language,
                LanguageLevel = LanguageLevel.Fluent,
            };

            Country country = new Country()
            {
                Name = "name"
            };

            City city = new City()
            {
                Country = country,
                Name = "dnepr"
            };

            Photo photo = new Photo()
            {
                Description = "descr",
                ImagePath = "path"
            };

            Skill skill = new Skill()
            {
                Title = "C#"
            };

            SocialNetwork socialNetwork = new SocialNetwork()
            {
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
                Experience = experience,
                Files = new List<File>() { candidateFile },
                Sources = new List<CandidateSource>() { candidateSource },
                FirstName = "TESTNAME",
                IsMale = true,
                LanguageSkills = new List<LanguageSkill>() { languageSkill },
                LastName = "lname",
                City = city,
                MiddleName = "mname",
                PhoneNumbers = new List<string>() { "+380978762352" },
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

            var jsonResult = _controller.Add(DTOService.ToDTO<Candidate, CandidateDTO>(candidate));
            var result = jsonResult as JsonResult<CandidateDTO>;

            Assert.AreEqual("TESTNAME", result.Content.FirstName);
            Assert.AreEqual(2, result.Content.Id);
        }


        [Test]
        [TestCase(1)]
        public async void Delete_Candidate_OK(int id)
        {
            var _controller = new CandidatesController(new DummyCandidateRepository(new DummyBotContext()));
            _controller.Request = new System.Net.Http.HttpRequestMessage();
            _controller.Configuration = new System.Web.Http.HttpConfiguration();

            var actionResponse = await _controller.Delete(id).ExecuteAsync(new System.Threading.CancellationToken());

            Assert.AreEqual(HttpStatusCode.OK, actionResponse.StatusCode);
        }

        [Test]
        [TestCase(1532)]
        public async void Delete_Candidate_NoContent(int id)
        {
            var _controller = new CandidatesController(new DummyCandidateRepository(new DummyBotContext()));
            _controller.Request = new System.Net.Http.HttpRequestMessage();
            _controller.Configuration = new System.Web.Http.HttpConfiguration();


            var actionResponse = await _controller.Delete(id).ExecuteAsync(new System.Threading.CancellationToken());


            Assert.AreEqual(HttpStatusCode.NoContent, actionResponse.StatusCode);
        }

        [Test]
        [TestCase(1, "CHANGEDNAMETEST")]
        [TestCase(1, "DRUG")]
        public void Put_Candidate_OK(int id, string testname)
        {
            var _controller = new CandidatesController(new DummyCandidateRepository(new DummyBotContext()));
            _controller.Request = new System.Net.Http.HttpRequestMessage();
            _controller.Configuration = new System.Web.Http.HttpConfiguration();

            var getResult = _controller.Get(id);
            var jsonGetResult = getResult as JsonResult<CandidateDTO>;
            var candidateDto = jsonGetResult.Content;

            candidateDto.FirstName = testname;

           /* var putResult = _controller.Put(candidateDto.Id, JObject.FromObject(candidateDto));
            var jsonPutResult = getResult as JsonResult<CandidateDTO>;
            var candidateAfterPut = jsonPutResult.Content;

            Assert.AreEqual(testname, candidateAfterPut.FirstName);*/
        }
    }
}
