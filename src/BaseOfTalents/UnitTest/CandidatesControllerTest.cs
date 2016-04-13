using Data.DumbData;
using Data.DumbData.Repositories;
using Domain.Entities;
using Domain.Entities.Enum;
using Domain.Entities.Setup;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebApi.Controllers;
using WebApi.DTO.DTOModels;
using AutoMapper;
using System.Net.Http;
using System.Diagnostics;

namespace UnitTest
{
    [TestFixture]
    class CandidatesControllerTest
    {
        [SetUp]
        public void StartTesting()
        {
            System.Diagnostics.Debugger.Launch();
        }
        [Test]
        public void Get_Candidate()
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
            var _controller = new CandidatesController(new DummyCandidateRepository(new DummyBotContext()));
            _controller.Request = new System.Net.Http.HttpRequestMessage();
            _controller.Configuration = new System.Web.Http.HttpConfiguration();

            var response = _controller.Get(1);

        }
    }
}
