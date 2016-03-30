using BotLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BotWebApi.DTOs.CandidateDTO
{
    public class SimplyfiedVacancyStageInfo
    {
        public int VacancyId { get; set; }
        public List<StageInfo> StageInfos { get; set; }
    }
}