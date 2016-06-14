using BaseOfTalents.Domain.Entities;
using Domain.DTO.DTOModels;

namespace DAL.Extensions
{
    public static class EventExtensions
    {
        public static void Update(this Event destination, EventDTO source)
        {
            destination.Description = source.Description;
            destination.EventDate = source.EventDate;
            destination.ResponsibleId = source.ResponsibleId;

            destination.CandidateId = source.CandidateId;
            destination.VacancyId = source.VacancyId;
            destination.EventTypeId = source.EventTypeId;
        }
    }
}
