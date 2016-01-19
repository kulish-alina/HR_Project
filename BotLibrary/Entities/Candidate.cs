using BotLibrary.Entities.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotLibrary.Entities
{
    public class Candidate: BaseEntity
    {
        PersonalInfo PersonalInfo { get; set; }
        ContactInfo ContactInfo { get; set; }
        WorkInfo WorkInfo { get; set; }
        Location Location { get; set; }
        bool RelocationAgreement { get; set; }
        List<SocialNetwork> SocialNetworks { get; set; }
        string Education { get; set; }
        List<Language> Languages { get; set; }
        Dictionary<Vacancy, StageInfo> VacanciesProgress { get; set; }
        List<Comment> Comments { get; set; }
    }
}
