using BaseOfTalents.DAL;
using BaseOfTalents.DAL.Infrastructure;
using BaseOfTalents.Domain.Entities.Enum.Setup;
using DAL.Services;
using Domain.DTO.DTOModels.SetupDTO;
using NUnit.Framework;
using System.Collections.Generic;
using System.Web.Http.Results;
using WebApi.Controllers;

namespace Tests.Controller
{
    [TestFixture]
    public class IndustryControllerTest : BaseTest
    {
        IndustryController controller;

        [SetUp]
        public void Init()
        {
            System.Diagnostics.Debug.WriteLine("SetUp");

            context = GenerateNewContext();

            context.Industries.AddRange(industries);
            context.SaveChanges();

            IUnitOfWork uow = new UnitOfWork(context);
            IndustryService service = new IndustryService(uow);

            controller = new IndustryController(service);
        }

        [TearDown]
        public void TearDown()
        {
            controller = null;
            context.Database.Delete();
            context = null;
        }

        //[Test]
        //public void OnGetControllerShouldReturnIndustriesCollection()
        //{
        //    System.Diagnostics.Debug.WriteLine("Test collection");
        //    var httpResult = controller.Get();
        //    var response = httpResult as JsonResult<IEnumerable<IndustryDTO>>;
        //    var collection = response.Content;

        //    Assert.IsNotNull(response);
        //    Assert.AreEqual(3, collection.Count());
        //}

        [Test]
        public void OnPostControllerShouldAddIndustryToDb()
        {
            System.Diagnostics.Debug.WriteLine("Test add");
            var dto = new IndustryDTO { Title = "test Industry" };

            var httpResult = controller.Add(dto);
            var response = httpResult as JsonResult<IndustryDTO>;
            var result = response.Content;

            Assert.IsNotNull(response);
            Assert.AreEqual(dto.Title, result.Title);
            Assert.AreNotEqual(0, result.Id);

        }

        List<Industry> industries = new List<Industry> {
                        new Industry {Id = 1, Title = "IT"},
                        new Industry {Id = 2,Title = "Accounting"},
                        new Industry {Id = 3,Title = "Administration"}};
    }
}