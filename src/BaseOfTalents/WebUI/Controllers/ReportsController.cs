using DAL.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Web.Http;
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
        [HttpGet]
        [Route("usersReport")]
        public IHttpActionResult GetDataForUserReport([FromUri]UsersReportParameters usersReportParams)
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

            var resultForPeriod = new { StartDate = usersReportParams.StartDate, EndDate = usersReportParams.EndDate, UserReport = usersReportResult };

            return Json(resultForPeriod, BOT_SERIALIZER_SETTINGS);
        }
        [HttpGet]
        [Route("candidateProgressReport")]
        public IHttpActionResult GetCandidateProgressReport([FromUri]CandidatesReportParameters candidatesReportParams)
        {
            if (!ModelState.IsValid || candidatesReportParams == null)
            {
                if (candidatesReportParams == null)
                {
                    ModelState.AddModelError("Request", "No params is listed");
                }
                return BadRequest(ModelState);
            }
            var report = service.GetCandidateProgressReport(candidatesReportParams.CandidatesIds,
                candidatesReportParams.LocationsIds,
                candidatesReportParams.StartDate,
                candidatesReportParams.EndDate);
            return Json(report, BOT_SERIALIZER_SETTINGS);
        }


        [HttpGet]
        [Route("vacanciesReport")]
        public IHttpActionResult GetDataForVacancyReport([FromUri]VacanciesReportParameters vacanciesReportParams)
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

                var resultForPeriod = new { StartDate = vacanciesReportParams.StartDate, EndDate = vacanciesReportParams.EndDate, StartDateReport = dailyReportForStartDate, VacanciesReport = vacanciesReportResult, EndDateReport = dailyReportForEndDate };
                return Json(resultForPeriod, BOT_SERIALIZER_SETTINGS);
            }
            else
            {
                var resultForDay = new { StartDate = vacanciesReportParams.StartDate, StartDateReport = dailyReportForStartDate };
                return Json(resultForDay, BOT_SERIALIZER_SETTINGS);
            }
        }
    }
}
