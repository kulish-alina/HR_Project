using BotLibrary.Entities.Enum;
using BotLibrary.Entities.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotLibrary.Entities
{
    public class Vacancy: BaseEntity 
    {
        string Name { get; set; }
        string Description { get; set; }
        CompanyDevision Division { get; set; }
        Location Location { get; set; }
        User Responsible { get; set; }
        List<Skill> RecuiredSkills { get; set; }
        List<Language> RecuiredLanguages { get; set; }
        TypeOfEmployment TypeOfEmployment { get; set; }
        DateTime StartDate { get; set; }
        DateTime EndDate { get; set; }
        VacancyStatus Status { get; set; }
        Dictionary<Candidate, StageInfo> CandidatesProgress { get; set; }
        List<Comment> Comments { get; set; }
    }
}
