using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class ChangePasswordModel
    {
        public string OldPassword { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 8)]
        public string NewPassword { get; set; }
    }
}