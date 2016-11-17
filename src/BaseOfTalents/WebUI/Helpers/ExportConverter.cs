using System;
using System.Collections.Generic;
using System.Linq;
using DAL.DTO.ReportDTO;
using DAL.DTO.SetupDTO;
using DAL.Services;
using Domain.Entities.Enum.Setup;
using Exporter;

namespace WebUI.Helpers
{
    public class ExportConverter : IConverter<Dictionary<int, List<CandidateProgressReportUnitDTO>>, ExportDataSet>
    {
        private BaseService<City, CityDTO> _cityService;
        private BaseService<Stage, StageDTO> _stageService;

        public ExportConverter(BaseService<City, CityDTO> cityService, BaseService<Stage, StageDTO> stageService)
        {
            _cityService = cityService;
            _stageService = stageService;
        }

        public ExportDataSet Convert(Dictionary<int, List<CandidateProgressReportUnitDTO>> report)
        {
            IEnumerable<City> locations = _cityService.Get(report.Keys);
            IEnumerable<Tuple<City, int>> loc = locations.Select(x => Tuple.Create(x, report[x.Id].Count));
            IOrderedEnumerable<StageDTO> stages = _stageService.Get().OrderBy(stage => stage.Order);
            IEnumerable<CandidateVacancyData> candidates = report.Values
                .SelectMany(x => x.Select(progressUnit => new CandidateVacancyData
                {
                    Candidate = $"{progressUnit.CandidateFirstName} {progressUnit.CandidateLastName}",
                    Vacancy = progressUnit.VacancyTitle
                }));

            IEnumerable<IEnumerable<StageInfoDTO>> temp = report.Values.SelectMany(x => x.Select(y => y.Stages));

            return new ExportDataSet(loc, stages, candidates, temp);
        }
    }

}