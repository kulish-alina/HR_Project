using System.IO;
using System.Web.Http;
using WebUI.Models.Reports;
using WebUI.Results;
using WebUI.Services;

namespace WebUI.Controllers
{
    [RoutePrefix("api/export")]
    public class ExportController : ApiController
    {
        private ExportService _service;

        public ExportController(ExportService service)
        {
            _service = service;
        }

        [Route("candidateProgressReport")]
        public IHttpActionResult GetExcelReport([FromUri]CandidatesReportParameters candidatesReportParams)
        {
            Stream result = _service.CandidateProgressExport(candidatesReportParams);
            string fileName = $"Report-{candidatesReportParams.StartDate}-{candidatesReportParams.EndDate}.xlsx";
            return new ExcelResult(fileName, result);
        }
    }
}