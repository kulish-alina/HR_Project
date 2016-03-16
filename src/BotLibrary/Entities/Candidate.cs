using BotLibrary.Entities.Setup;
using System.Collections.Generic;

namespace BotLibrary.Entities
{
    public class Candidate: BaseEntity
    {
        public PersonalInfo PersonalInfo { get; set; }
        public ContactInfo ContactInfo { get; set; }
        public WorkInfo WorkInfo { get; set; }
        public string Description { get; set; }
        public Location Location { get; set; }
        public bool RelocationAgreement { get; set; }
        public List<SocialNetwork> SocialNetworks { get; set; }
        public string Education { get; set; }
        public List<Language> Languages { get; set; }
        public List<File> Files { get; set; }
        public Dictionary<Vacancy, StageInfo> VacanciesProgress { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Source> Sources { get; set; }
    }
}
