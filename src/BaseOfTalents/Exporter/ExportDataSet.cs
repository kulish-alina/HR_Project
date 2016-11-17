using System;
using System.Collections.Generic;
using DAL.DTO.ReportDTO;
using DAL.DTO.SetupDTO;
using Domain.Entities.Enum.Setup;

namespace Exporter
{
    public class ExportDataSet
    {
        public ExportDataSet(IEnumerable<Tuple<City, int>> locations, IEnumerable<StageDTO> stages,
            IEnumerable<CandidateVacancyData> candidateVacancyData,
            IEnumerable<IEnumerable<StageInfoDTO>> stagesFromReport)
        {
            Locations = locations;
            Stages = stages;
            CandidateVanancyData = candidateVacancyData;
            StagesFromReport = stagesFromReport;
        }

        public IEnumerable<Tuple<City, int>> Locations { get; private set; }
        public IEnumerable<StageDTO> Stages { get; private set; }

        public IEnumerable<CandidateVacancyData> CandidateVanancyData { get; private set; }

        public IEnumerable<IEnumerable<StageInfoDTO>> StagesFromReport { get; private set; }
    }
}
