using System.Collections.Generic;

namespace BotLibrary.Entities
{
    public class VacancyStageInfo
    {
        public Vacancy Vacancy { get; set; }
        public List<StageInfo> StageInfos { get; set; }
    }
}