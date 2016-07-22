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
    [TestFixture(Category = "Candidate updating")]
    public class CandidateUpdatingTests : BaseTest
    {
        CandidateController controller;

        [SetUp]
        public void Init()
        {
            System.Diagnostics.Debug.WriteLine("candidates init");

            context = GenerateNewContext();

            context.Tags.AddRange(DummyData.Tags);
            context.Currencies.AddRange(DummyData.Currencies);
            context.Skills.AddRange(DummyData.Skills);
            context.EventTypes.AddRange(DummyData.EventTypes);
            context.SocialNetworks.AddRange(DummyData.Socials);
            context.Tags.AddRange(DummyData.Tags);
            context.Industries.AddRange(DummyData.Industries);
            context.Levels.AddRange(DummyData.Levels);
            context.DepartmentGroups.AddRange(DummyData.DepartmentGroups);
            context.Departments.AddRange(DummyData.Departments);
            context.Languages.AddRange(DummyData.Languages);
            context.LanguageSkills.AddRange(DummyData.LanguageSkills);
            context.Countries.AddRange(DummyData.Countries);
            context.Cities.AddRange(DummyData.Cities);
            context.Stages.AddRange(DummyData.Stages);
            context.Permissions.AddRange(DummyData.Permissions);
            context.Roles.AddRange(DummyData.Roles);
            context.Users.Add(DummyData.Users.First());
            context.Sources.AddRange(DummyData.Sources);
            context.Vacancies.Add(DummyData.Vacancies.First());
            context.SaveChanges();

            var candidate = new Candidate
            {
                CityId = 1,
                BirthDate = new DateTime(1980, 1, 1),
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
                PhoneNumbers = new List<PhoneNumber>() { new PhoneNumber { Number = "+38098989898" } },
                Photo = new File { Description = "description", FilePath = "description" },
                PositionDesired = "position",
                Practice = "practice",
                RelocationAgreement = false,
                SalaryDesired = 3000,
                Skills = new List<Skill> { DummyData.Skills.First() },
                Skype = "skyper",
                SocialNetworks = new List<CandidateSocial>() { new CandidateSocial { SocialNetworkId = 1, Path = "path" } },
                Sources = new List<CandidateSource> { },
                StartExperience = new DateTime(2000, 1, 1),
                Tags = new List<Tag>() { DummyData.Tags.First() },
                TypeOfEmployment = TypeOfEmployment.FullTime,
                Level = DummyData.Levels.First(),
                VacanciesProgress = new List<VacancyStageInfo>(),
                Events = new List<Event> { new Event { EventDate = new DateTime(2001, 1, 1), CandidateId = 1, EventTypeId = 1, Description = "someDescr", ResponsibleId = 1 } }
            };



            int vacancyId = context.Vacancies.First().Id;
            candidate.VacanciesProgress.Add(new VacancyStageInfo
            {

            });
            candidate.Sources.Add(new CandidateSource { Candidate = candidate, Path = "path", SourceId = context.Sources.First().Id });

            context.Candidates.Add(candidate);

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

        /*[Test]
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
        */
        [Test]
        public void ShouldAddSources()
        {
            var httpResult = controller.Get(1);
            var response = httpResult as JsonResult<CandidateDTO>;
            var candidate = response.Content;

            var candidateSource = candidate.Sources.First();

            string newPath = "path";
            candidateSource.Path = newPath;

            var newHttpResult = controller.Put(candidate.Id, candidate);
            var newResponse = newHttpResult as JsonResult<CandidateDTO>;
            var newCandidate = newResponse.Content;

            Assert.IsTrue(newCandidate.Sources.Any(x => x.Path == newPath));
        }

        [Test]
        public void ShouldAddPhoneNumbers()
        {
            var httpResult = controller.Get(1);
            var response = httpResult as JsonResult<CandidateDTO>;
            var candidate = response.Content;

            var phoneNumber = candidate.PhoneNumbers.ToList().First();

            string newNumber = "+3890000000";
            phoneNumber.Number = newNumber;

            var newHttpResult = controller.Put(candidate.Id, candidate);
            var newResponse = newHttpResult as JsonResult<CandidateDTO>;
            var newCandidate = newResponse.Content;

            Assert.IsTrue(newCandidate.PhoneNumbers.Any(x => x.Number == newNumber));
        }

        [Test]
        public void ShouldUpdateVacancyProgress()
        {
            /*var httpResult = controller.Get(1);
            var response = httpResult as JsonResult<CandidateDTO>;
            var candidate = response.Content;

            var vacanciesProgress = candidate.VacanciesProgress.ToList().First();

            vacanciesProgress.VacancyStage.StageId = 2;
            vacanciesProgress.VacancyStage.Order = 2;

            var newHttpResult = controller.Put(candidate.Id, candidate);
            var newResponse = newHttpResult as JsonResult<CandidateDTO>;
            var newCandidate = newResponse.Content;

            Assert.IsTrue(newCandidate.VacanciesProgress.Any(x => x.VacancyStage.StageId == 2 && x.VacancyStage.Order == 2));*/
        }

        [Test]
        public void ShouldUpdateTags()
        {
            var httpResult = controller.Get(1);
            var response = httpResult as JsonResult<CandidateDTO>;
            var candidate = response.Content;

            var newTagId = 2;

            var tagids = candidate.TagIds.ToList();
            tagids.Remove(tagids.First());
            tagids.Add(newTagId);
            candidate.TagIds = tagids;

            var newHttpResult = controller.Put(candidate.Id, candidate);
            var newResponse = newHttpResult as JsonResult<CandidateDTO>;
            var newCandidate = newResponse.Content;

            Assert.IsTrue(newCandidate.TagIds.Any(x => x == newTagId));
        }

        [Test]
        public void ShouldUpdateSkills()
        {
            var httpResult = controller.Get(1);
            var response = httpResult as JsonResult<CandidateDTO>;
            var candidate = response.Content;

            var newSkillId = 2;

            var skillsIds = candidate.SkillIds.ToList();
            skillsIds.Remove(skillsIds.First());
            skillsIds.Add(newSkillId);
            candidate.SkillIds = skillsIds;

            var newHttpResult = controller.Put(candidate.Id, candidate);
            var newResponse = newHttpResult as JsonResult<CandidateDTO>;
            var newCandidate = newResponse.Content;

            Assert.IsTrue(newCandidate.SkillIds.Any(x => x == newSkillId));
        }

        [Test]
        public void ShouldUpdateComments()
        {
            var httpResult = controller.Get(1);
            var response = httpResult as JsonResult<CandidateDTO>;
            var candidate = response.Content;

            var newCommentMessage = "some another message";

            var oldComment = candidate.Comments.First();
            oldComment.Message = newCommentMessage;

            var newHttpResult = controller.Put(candidate.Id, candidate);
            var newResponse = newHttpResult as JsonResult<CandidateDTO>;
            var newCandidate = newResponse.Content;

            Assert.IsTrue(newCandidate.Comments.Any(x => x.Message == newCommentMessage));
        }

        [Test]
        public void ShouldUpdateEvent()
        {
            var httpResult = controller.Get(1);
            var response = httpResult as JsonResult<CandidateDTO>;
            var candidate = response.Content;

            var newResponsible = context.Users.First(x => x.Id != 1).Id;
            var newDescription = "someventDescription";
            var newDate = new DateTime(2001, 2, 2);

            var evnt = candidate.Events.First();
            evnt.Description = newDescription;
            evnt.ResponsibleId = newResponsible;
            evnt.EventDate = newDate;

            var newHttpResult = controller.Put(candidate.Id, candidate);
            var newResponse = newHttpResult as JsonResult<CandidateDTO>;
            var newCandidate = newResponse.Content;

            Assert.IsTrue(newCandidate.Events.Any(x => x.Description == newDescription && x.ResponsibleId == newResponsible && x.EventDate == newDate));
        }


    }
}
