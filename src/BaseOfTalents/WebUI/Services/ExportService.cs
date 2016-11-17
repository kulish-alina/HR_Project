using System.Collections.Generic;
using System.IO;
using DAL.DTO.ReportDTO;
using DAL.Services;
using Exporter;
using WebUI.Helpers;
using WebUI.Models.Reports;

namespace WebUI.Services
{
    public class ExportService
    {
        private ReportService _reportService;
        private IConverter<Dictionary<int, List<CandidateProgressReportUnitDTO>>, ExportDataSet> _converter;
        private IExporter<ExportDataSet> _exporter;

        public ExportService(ReportService reportService,
            IConverter<Dictionary<int, List<CandidateProgressReportUnitDTO>>, ExportDataSet> converter,
            IExporter<ExportDataSet> exporter)
        {
            _reportService = reportService;
            _converter = converter;
            _exporter = exporter;
        }

        public Stream CandidateProgressExport(CandidatesReportParameters param)
        {
            Dictionary<int, List<CandidateProgressReportUnitDTO>> report = _reportService.GetCandidateProgressReport(
               param.CandidatesIds,
               param.LocationsIds,
               param.StartDate,
               param.EndDate);

            ExportDataSet data = _converter.Convert(report);
            return _exporter.Export(data);
        }
    }
}