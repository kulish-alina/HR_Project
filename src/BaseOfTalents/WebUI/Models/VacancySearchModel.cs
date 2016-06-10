using System;

namespace BaseOfTalents.WebUI.Models
{
    public sealed class VacancySearchModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int State { get; set; }
        public int LocationId { get; set; }
        public int ResponsibleId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}