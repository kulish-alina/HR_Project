using BaseOfTalents.DAL;
using BaseOfTalents.DAL.Infrastructure;
using BaseOfTalents.DAL.Migrations;
using BaseOfTalents.Domain.Entities;
using BaseOfTalents.Domain.Entities.Enum;
using BaseOfTalents.Domain.Entities.Enum.Setup;
using BaseOfTalents.WebUI.Controllers;
using DAL.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests.Controllers
{
    public class CandidateControllerTests : BaseTest
    {
        CandidateController controller;

        [SetUp]
        public void Init()
        {
            System.Diagnostics.Debug.WriteLine("candidates init");

            //Debugger.Launch();
            context.LanguageSkills.AddRange(DummyData.LanguageSkills);
            context.SaveChanges();

            context.Candidates.AddRange(candidates);
            context.SaveChanges();

            context.EventTypes.AddRange(DummyData.EventTypes);
            context.SaveChanges();

            context.Users.AddRange(DummyData.Users);
            context.SaveChanges();

            IUnitOfWork uow = new UnitOfWork(context);
            CandidateService service = new CandidateService(uow);

            controller = new CandidateController(service);
        }

        [TearDown]
        public void TearDown()
        {
            System.Diagnostics.Debug.WriteLine("candidates teardown");
            controller = null;
        }

        /*[Test]
        public void OnUpdatingCandidateControllerShouldUpdateEvents()
        {
            var httpResult = controller.Get(1);
            var response = httpResult as JsonResult<CandidateDTO>;
            var candidate = response.Content;

            var respId = context.Users.First().Id;
            var eventDescr = "someventDescription";

            candidate.Events = new List<EventDTO> { new EventDTO { EventDate = new DateTime(2001, 1, 1), CandidateId = 1, EventTypeId = 1, Description = eventDescr, ResponsibleId = respId } };

            var newHttpResult = controller.Put(candidate.Id, candidate);
            var newResponse = newHttpResult as JsonResult<CandidateDTO>;
            var newCandidate = newResponse.Content;

            Assert.AreEqual(newCandidate.Events.First().ResponsibleId, respId);
            Assert.AreEqual(newCandidate.Events.First().CandidateId, 1);
            Assert.AreEqual(newCandidate.Events.First().Description, eventDescr);
        }*/

        List<Candidate> candidates = new List<Candidate>
        {
            new Candidate
            {
                    CityId = 1,
                    BirthDate = new DateTime(1980, 1,1),
                    Comments = new List<Comment> { new Comment { Message = "someComment" } },
                    Education = "education",
                    FirstName = "first name",
                    IndustryId = 1,
                    Description = "description",
                    Email = "email@email.com",
                    Files = new List<File>(),
                    IsMale = true,
                    LanguageSkills = new List<LanguageSkill> { new LanguageSkill { LanguageId = 1, LanguageLevel = null } },
                    LastName = "last name",
                    MiddleName = "middlename",
                    PhoneNumbers = new List<PhoneNumber>(),
                    Photo = new Photo { Description = "description", ImagePath = "description" },
                    PositionDesired = "position",
                    Practice = "practice",
                    RelocationAgreement = false,
                    SalaryDesired = 3000,
                    Skills = new List<Skill> {  DummyData.Skills.First() },
                    Skype = "skyper",
                    //SocialNetworks = new List<CandidateSocial>() { new CandidateSocial() {SocialNetwork = Socials.GetRandom(), Path = GetRandomString(15) } },
                    Sources = new List<CandidateSource> { DummyData.CandidateSources.First() },
                    StartExperience = new DateTime(2000, 1,1),
                    Tags = new List<Tag>(),
                    TypeOfEmployment = TypeOfEmployment.FullTime,
                    Level = DummyData.Levels.First(),
                    VacanciesProgress = new List<VacancyStageInfo>(),
            }
        };
    }
}
