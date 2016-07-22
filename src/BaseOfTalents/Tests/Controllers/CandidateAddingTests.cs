using DAL;
using DAL.DTO;
using DAL.Infrastructure;
using DAL.Migrations;
using DAL.Services;
using Domain.Entities;
using Domain.Entities.Enum;
using Domain.Entities.Enum.Setup;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Results;
using WebUI.Controllers;

namespace Tests.Controllers
{
    [TestFixture(Category = "Candidate adding")]
    public partial class CandidateAddingTests : BaseTest
    {
        CandidateController controller;

        [SetUp]
        public void Init()
        {
            System.Diagnostics.Debug.WriteLine("candidates init");

            context = GenerateNewContext();

            context.Sources.AddRange(DummyData.Sources);
            context.Countries.AddRange(DummyData.Countries);
            context.Languages.AddRange(DummyData.Languages);
            context.SocialNetworks.AddRange(DummyData.Socials);
            context.Tags.AddRange(DummyData.Tags);
            context.LanguageSkills.AddRange(DummyData.LanguageSkills);
            context.Cities.AddRange(DummyData.Cities);
            context.Industries.AddRange(DummyData.Industries);
            context.EventTypes.AddRange(DummyData.EventTypes);
            context.Users.AddRange(DummyData.Users);
            context.Candidates.AddRange(candidates);
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
            context.Database.Delete();
            context = null;
        }

        [Test]
        public void ShouldAddRelocationPlaces()
        {
            var httpResult = controller.Get(1);
            var response = httpResult as JsonResult<CandidateDTO>;
            var candidate = response.Content;

            int countryId = context.Countries.First().Id;
            int cityId = context.Cities.First().Id;

            var newRelocationPlace = new RelocationPlaceDTO
            {
                CountryId = countryId,
                CityId = cityId
            };

            var places = candidate.RelocationPlaces.ToList();
            places.Add(newRelocationPlace);
            candidate.RelocationPlaces = places;

            var newHttpResult = controller.Put(candidate.Id, candidate);
            var newResponse = newHttpResult as JsonResult<CandidateDTO>;
            var newCandidate = newResponse.Content;

            Assert.IsTrue(newCandidate.RelocationPlaces.Any(x => x.CityId == cityId && x.CountryId == countryId));
        }

        [Test]
        public void ShouldAddCandidateSocials()
        {
            var httpResult = controller.Get(1);
            var response = httpResult as JsonResult<CandidateDTO>;
            var candidate = response.Content;

            int socialId = context.SocialNetworks.First().Id;
            var path = "TESTPATH";

            var newCandidateSocial = new CandidateSocialDTO
            {
                SocialNetworkId = socialId,
                Path = path
            };

            var socials = candidate.SocialNetworks.ToList();
            socials.Add(newCandidateSocial);
            candidate.SocialNetworks = socials;

            var newHttpResult = controller.Put(candidate.Id, candidate);
            var newResponse = newHttpResult as JsonResult<CandidateDTO>;
            var newCandidate = newResponse.Content;

            Assert.IsTrue(newCandidate.SocialNetworks.Any(x => x.SocialNetworkId == socialId && x.Path == path));
        }

        [Test]
        public void ShouldAddLanguageSkills()
        {
            var httpResult = controller.Get(1);
            var response = httpResult as JsonResult<CandidateDTO>;
            var candidate = response.Content;

            int languageId = context.Languages.First().Id;
            LanguageLevel languageLevel = LanguageLevel.Advanced;

            var newLanguageSkill = new LanguageSkillDTO
            {
                LanguageId = languageId,
                LanguageLevel = languageLevel
            };

            var languageSkills = candidate.LanguageSkills.ToList();
            languageSkills.Add(newLanguageSkill);
            candidate.LanguageSkills = languageSkills;

            var newHttpResult = controller.Put(candidate.Id, candidate);
            var newResponse = newHttpResult as JsonResult<CandidateDTO>;
            var newCandidate = newResponse.Content;

            Assert.IsTrue(newCandidate.LanguageSkills.Any(x => x.LanguageId == languageId && x.LanguageLevel == languageLevel));
        }

        [Test]
        public void ShouldAddSources()
        {
            var httpResult = controller.Get(1);
            var response = httpResult as JsonResult<CandidateDTO>;
            var candidate = response.Content;

            int source = context.Sources.First().Id;

            var newCandidateSource = new CandidateSourceDTO
            {
                Path = "candidate social path",
                SourceId = source
            };

            var sources = candidate.Sources.ToList();
            sources.Add(newCandidateSource);
            candidate.Sources = sources;

            var newHttpResult = controller.Put(candidate.Id, candidate);
            var newResponse = newHttpResult as JsonResult<CandidateDTO>;
            var newCandidate = newResponse.Content;

            Assert.IsTrue(newCandidate.Sources.Any(x => x.Path == "candidate social path" && x.SourceId == source));
        }

        [Test]
        public void ShouldAddPhoneNumbers()
        {
            var httpResult = controller.Get(1);
            var response = httpResult as JsonResult<CandidateDTO>;
            var candidate = response.Content;

            var newPhoneNumber = new PhoneNumberDTO { Number = "+380930986252" };
            var phoneNumbers = candidate.PhoneNumbers.ToList();
            phoneNumbers.Add(newPhoneNumber);
            candidate.PhoneNumbers = phoneNumbers;

            var newHttpResult = controller.Put(candidate.Id, candidate);
            var newResponse = newHttpResult as JsonResult<CandidateDTO>;
            var newCandidate = newResponse.Content;

            Assert.IsTrue(newCandidate.PhoneNumbers.Any(x => x.Number == "+380930986252"));
        }

        [Test]
        public void ShouldAddVacancyProgress()
        {
            /*var httpResult = controller.Get(1);
            var response = httpResult as JsonResult<CandidateDTO>;
            var candidate = response.Content;

            context.Vacancies.Add(DummyData.Vacancies.GetRandom());
            context.SaveChanges();

            context.Stages.AddRange(DummyData.Stages);
            context.SaveChanges();

            int vacancyId = context.Vacancies.First().Id;

            var newVacancyProgress = new VacancyStageInfoDTO
            {
                CandidateId = 1,
                VacancyId = vacancyId,
                VacancyStage = new VacancyStageDTO
                {
                    IsCommentRequired = false,
                    Order = 1,
                    StageId = 1
                }
            };

            var vacanciesProgress = candidate.VacanciesProgress.ToList();
            vacanciesProgress.Add(newVacancyProgress);
            candidate.VacanciesProgress = vacanciesProgress;

            var newHttpResult = controller.Put(candidate.Id, candidate);
            var newResponse = newHttpResult as JsonResult<CandidateDTO>;
            var newCandidate = newResponse.Content;

            Assert.IsTrue(newCandidate.VacanciesProgress.Any(x => x.VacancyId == vacancyId && x.CandidateId == 1 && x.VacancyStage.StageId == 1 && x.VacancyStage.IsCommentRequired == false));
       */
        }

        [Test]
        public void ShouldAddTag()
        {
            var httpResult = controller.Get(1);
            var response = httpResult as JsonResult<CandidateDTO>;
            var candidate = response.Content;

            var newTagId = 1;

            candidate.TagIds = new List<int>() { newTagId };

            var newHttpResult = controller.Put(candidate.Id, candidate);
            var newResponse = newHttpResult as JsonResult<CandidateDTO>;
            var newCandidate = newResponse.Content;

            Assert.IsTrue(newCandidate.TagIds.Any(x => x == newTagId));
        }

        [Test]
        public void ShouldAddSkills()
        {
            var httpResult = controller.Get(1);
            var response = httpResult as JsonResult<CandidateDTO>;
            var candidate = response.Content;

            var newSkillId = 1;

            candidate.SkillIds = new List<int>() { newSkillId };

            var newHttpResult = controller.Put(candidate.Id, candidate);
            var newResponse = newHttpResult as JsonResult<CandidateDTO>;
            var newCandidate = newResponse.Content;

            Assert.IsTrue(newCandidate.SkillIds.Any(x => x == newSkillId));
        }

        [Test]
        public void ShouldAddComments()
        {
            var httpResult = controller.Get(1);
            var response = httpResult as JsonResult<CandidateDTO>;
            var candidate = response.Content;

            var newComment = new CommentDTO { Message = "Message" };

            var newCommentsList = candidate.Comments.ToList();
            newCommentsList.Add(newComment);
            candidate.Comments = newCommentsList;

            var newHttpResult = controller.Put(candidate.Id, candidate);
            var newResponse = newHttpResult as JsonResult<CandidateDTO>;
            var newCandidate = newResponse.Content;

            Assert.IsTrue(newCandidate.Comments.Any(x => x.Message == "Message"));
        }

        [Test]
        public void ShouldAddEvents()
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
        }

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
                    Photo = new File { Description = "description", FilePath = "description" },
                    PositionDesired = "position",
                    Practice = "practice",
                    RelocationAgreement = false,
                    SalaryDesired = 3000,
                    Skills = new List<Skill> {  DummyData.Skills.First() },
                    Skype = "skyper",
                    //SocialNetworks = new List<CandidateSocial>() { new CandidateSocial() {SocialNetwork = Socials.GetRandom(), Path = GetRandomString(15) } },
                    //Sources = new List<CandidateSource> { DummyData.CandidateSources.First() },
                    StartExperience = new DateTime(2000, 1,1),
                    Tags = new List<Tag>(),
                    TypeOfEmployment = TypeOfEmployment.FullTime,
                    Level = DummyData.Levels.First(),
                    VacanciesProgress = new List<VacancyStageInfo>(),
            }
        };
    }
}
