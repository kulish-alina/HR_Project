using Autofac.Integration.WebApi;
using Data.Infrastructure;
using Data.Migrations;
using Service.Services;
using Domain.DTO.DTOModels;
using Domain.Entities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using WebApi;

namespace UnitTest
{
    [TestFixture]
    public class VacancyServiceTDD
    {
        private HttpConfiguration _configuration = null;
        [TestFixtureSetUp]
        public void SetUp()
        {
            this._configuration = new HttpConfiguration();
            AutofacTESTConfiguration.Initialize(_configuration);
            AutoMapperWebConfiguration.Configure();
        }

        [Test]
        public void VacancyServiceShouldReturnVacancyById()
        {
            //int vacancyId = 1;
            //var request = new HttpRequestMessage();

            //var vacancyService = (IControllerService<Vacancy, VacancyDTO>)_configuration.DependencyResolver.GetService(typeof(IControllerService<Vacancy, VacancyDTO>));
            //var vacancy = vacancyService.GetById(vacancyId);

            //Assert.AreEqual(vacancy.Id, vacancyId);
        }
    }
}
