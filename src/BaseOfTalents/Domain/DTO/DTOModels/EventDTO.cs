using System;

namespace Domain.DTO.DTOModels
{
    public class EventDTO: BaseEntityDTO
    {
        public DateTime EventDate { get; set; }
        public string Description { get; set; }

        public int EventTypeId { get; set; }

        public VacancyDTO Vacancy { get; set; }
        public CandidateDTO Candidate { get; set; }

        public int ResponsibleId { get; set; }
    }
}
