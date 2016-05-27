using Data.Infrastructure;
using Service.Services;
using Domain.DTO.DTOModels.SetupDTO;
using Domain.Entities.Setup;
using Domain.Repositories;

namespace WebApi.Controllers
{
    public class EventTypesController : BoTController<EventType, EventTypeDTO>
    {
        public EventTypesController(IControllerService<EventType, EventTypeDTO> service)
            : base(service)
        {
        }
    }
}