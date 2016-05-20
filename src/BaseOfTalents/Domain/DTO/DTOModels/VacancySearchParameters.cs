using Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.DTOModels
{
    public class VacancySearchParameters
    {
        public VacancySearchParameters()
        {
            LevelIds = new List<int>();
            LocationIds = new List<int>();
            Current = 1;
            Size = 20;
        }
        public int? UserId { get; set; }
        public int? IndustryId { get; set; }
        public string Title { get; set; }

        public EntityState? VacancyState { get; set; }
        public TypeOfEmployment? TypeOfEmployment { get; set; }

        public IEnumerable<int> LevelIds { get; set; }
        public IEnumerable<int> LocationIds { get; set; }

        public int Current { get; set; }
        public int Size { get; set; }
    }
}
