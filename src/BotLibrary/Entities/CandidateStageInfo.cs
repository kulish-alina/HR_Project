using System.Collections.Generic;

namespace BotLibrary.Entities
{
    public class CandidateStageInfo
    {
        public Candidate Candidate { get; set; }
        public List<StageInfo> StageInfos { get; set; }
    }
}