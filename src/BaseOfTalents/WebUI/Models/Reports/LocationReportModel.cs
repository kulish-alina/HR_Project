using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models.Reports
{
    public class LocationReportModel
    {
        public LocationReportModel()
        {
            UserModels = new List<UserReportModel>();
        }
        public string Location { get; set; }
        public ICollection<UserReportModel> UserModels { get; set; }
    }
}