using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.DTO
{
    public class EventDTO : BaseEntityDTO
    {
        [Required]
        public DateTime EventDate { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int ResponsibleId { get; set; }

        public int? EventTypeId { get; set; }
        public int? VacancyId { get; set; }
        public int? CandidateId { get; set; }
    }
}
