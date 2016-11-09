using System;
using System.Collections.Generic;

namespace WebUI.Models.Reports
{
    public class CandidatesReportParameters
    {
        public IEnumerable<int> CandidatesIds { get; set; }
        public IEnumerable<int> LocationsIds { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}