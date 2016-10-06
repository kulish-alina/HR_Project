using System;
using System.Collections.Generic;
using DAL.Services;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WebUI.Models.Reports;

namespace WebUI.Controllers
{
    [RoutePrefix("api/report")]
    public class ReportsController : ApiController
    {
        private ReportService service;
        private static JsonSerializerSettings BOT_SERIALIZER_SETTINGS = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
        public ReportsController(ReportService service)
        {
            this.service = service;
        }
        [HttpPost]
        [Route("usersReport")]
        public IHttpActionResult GetDataForUserReport([FromBody]UsersReportParameters usersReportParams)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usersReportResult = service.GetUsersReportData(
                usersReportParams.LocationIds,
                usersReportParams.UserIds,
                usersReportParams.StartDate,
                usersReportParams.EndDate
            );

            var resultForPeriod = new { StartDate = usersReportParams.StartDate, EndDate = usersReportParams.EndDate, UserReport = usersReportResult};

            return Json(resultForPeriod, BOT_SERIALIZER_SETTINGS);
        }

        [HttpPost]
        [Route("vacanciesReport")]
        public IHttpActionResult GetDataForVacancyReport([FromBody]VacanciesReportParameters vacanciesReportParams)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dailyReportForStartDate = service.GetDailyVacanciesReportData(
                vacanciesReportParams.LocationIds,
                vacanciesReportParams.UserIds,
                vacanciesReportParams.StartDate
            );
            if (vacanciesReportParams.EndDate != null)
            {
                var vacanciesReportResult = service.GetVacanciesReportData(
                    vacanciesReportParams.LocationIds,
                    vacanciesReportParams.UserIds,
                    vacanciesReportParams.StartDate,
                    vacanciesReportParams.EndDate
                    );
                var dailyReportForEndDate = service.GetDailyVacanciesReportData(
                    vacanciesReportParams.LocationIds,
                    vacanciesReportParams.UserIds,
                    vacanciesReportParams.EndDate
                    );

                var resultForPeriod = new {StartDate = vacanciesReportParams.StartDate, EndDate = vacanciesReportParams.EndDate,  StartDateReport = dailyReportForStartDate, VacanciesReport = vacanciesReportResult, EndDateReport = dailyReportForEndDate };
                return Json(resultForPeriod, BOT_SERIALIZER_SETTINGS);
            }
            else
            {
                var resultForDay = new { StartDateReport = dailyReportForStartDate };
                return Json(resultForDay, BOT_SERIALIZER_SETTINGS);
            }
        }
    }
}
