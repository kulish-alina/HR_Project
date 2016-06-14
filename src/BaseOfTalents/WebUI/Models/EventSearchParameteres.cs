using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BaseOfTalents.WebUI.Models
{
    public class EventSearchParameteres
    {
        [Required]
        public int Year { get; set; }
        [Required]
        public int Month { get; set; }
        [Required]
        public IEnumerable<int> UserIds { get; set; }
    }
}