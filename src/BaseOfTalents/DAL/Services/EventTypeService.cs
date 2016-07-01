using DAL.DTO.SetupDTO;
using DAL.Infrastructure;
using Domain.Entities.Enum.Setup;

namespace DAL.Services
{
    public class EventTypeService : BaseService<EventType, EventTypeDTO>
    {
        public EventTypeService(IUnitOfWork uow) : base(uow, uow.EventTypeRepo)
        {

        }
    }
}
