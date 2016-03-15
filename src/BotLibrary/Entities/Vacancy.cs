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
        Level Level {get; set;}
        string Description { get; set; }
        Department Department { get; set; }
        Location Location { get; set; }
        User Responsible { get; set; }
        List<Skill> RequiredSkills { get; set; }
        int SalaryMin { get; set; }
        int SalaryMax { get; set; }
        List<Language> RequiredLanguages { get; set; }
        TypeOfEmployment TypeOfEmployment { get; set; }
        DateTime StartDate { get; set; }
        DateTime EndDate { get; set; }
        DateTime DeadlineDate { get; set; }
        VacancyStatus Status { get; set; }
        Dictionary<Candidate, List<StageInfo>> CandidatesProgress { get; set; }
        bool MasterVacancy { get; set; }
        List<File> Files { get; set; }
        int ChildredVacanciesCount {get; set;}
        bool IsDeadlineAddedToCalendar { get; set; }
        List<Comment> Comments { get; set; }
    }
}
