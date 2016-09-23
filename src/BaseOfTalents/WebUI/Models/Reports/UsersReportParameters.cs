using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public ICollection<int> LocationIds { get; set; }
        public ICollection<int> UserIds { get; set; }
    }
}