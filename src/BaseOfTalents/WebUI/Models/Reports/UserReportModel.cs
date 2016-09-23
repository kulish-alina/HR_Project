using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models.Reports
{
    public class UserReportModel
    {
        public UserReportModel()
        {
            StagesDict = new Dictionary<string, int?>();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Added { get; set; }
        public Dictionary<string, int?> StagesDict { get; set; }
    }
}