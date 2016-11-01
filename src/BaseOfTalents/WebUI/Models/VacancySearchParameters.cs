using System.Collections.Generic;

namespace WebUI.Models
{
    public class VacancySearchParameters
    {
        public VacancySearchParameters()
        {
            LevelIds = new List<int>();
            CityIds = new List<int>();
            Current = 0;
            Size = 20;
        }

        public int? UserId { get; set; }
        public int? IndustryId { get; set; }
        public string Title { get; set; }

        public int? VacancyState { get; set; }
        public int? TypeOfEmployment { get; set; }

        public IEnumerable<int> LevelIds { get; set; }
        public IEnumerable<int> CityIds { get; set; }

        public int Current { get; set; }
        public int Size { get; set; }

        public string SortBy { get; set; }
        public bool? SortAsc { get; set; }

        public int? DepartmentId { get; set; }
    }
}