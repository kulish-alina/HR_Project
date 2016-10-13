using DAL.DTO;
using System;
using System.Collections.Generic;

namespace WebUI.Models
{
    public class CandidateSearchModel
    {
        public CandidateSearchModel()
        {
            LanguageSkills = new List<LanguageSkillDTO>();
            CitiesIds = new List<int>();
            Current = 0;
            Size = 20;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public bool? RelocationAgreement { get; set; }
        public bool? IsMale { get; set; }
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }
        public DateTime? StartExperience { get; set; }
        public int? MinSalary { get; set; }
        public int? MaxSalary { get; set; }
        public int? CurrencyId { get; set; }
        public int? IndustryId { get; set; }
        public string Position { get; set; }
        public string Technology { get; set; }

        public IEnumerable<LanguageSkillDTO> LanguageSkills { get; set; }
        public IEnumerable<int> CitiesIds { get; set; }

        public string SortBy { get; set; }
        public bool? SortAsc { get; set; }
        public string SearchString { get; set; }

        public int Current { get; set; }
        public int Size { get; set; }
    }
}