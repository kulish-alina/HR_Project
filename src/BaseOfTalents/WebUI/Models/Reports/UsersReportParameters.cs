using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models.Reports
{
    public class UsersReportParameters
    {
        public UsersReportParameters()
        {
            LocationIds = new List<int>();
            UserIds = new List<int>();
        }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<int> LocationIds { get; set; }
        public ICollection<int> UserIds { get; set; }
    }
}