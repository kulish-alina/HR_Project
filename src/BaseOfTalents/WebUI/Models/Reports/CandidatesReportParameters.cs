using System;
using System.Collections.Generic;

namespace WebUI.Models.Reports
{
    public class CandidatesReportParameters
    {
        public IEnumerable<int> CandidatesIds { get; set; }
        public IEnumerable<int> LocationsIds { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}