using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class VacancyStageModel
    {
        public VacancyStageModel VacancyStage { get; set; }
        public int CandidateId { get; set; }
        public int VacancyId { get; set; }
        public CommentModel Comment { get; set; }
    }
}