using BaseOfTalents.Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class LanguageSkillModel
    {
        public int LanguageId { get; set; }
        public LanguageLevel LanguageLevel { get; set; }
    }
}