using BaseOfTalents.Domain.Entities.Enum.Setup;
using DAL.Services;
using Domain.DTO.DTOModels.SetupDTO;
using System.Web.Http;

namespace WebApi.Controllers
{
    [RoutePrefix("api/EventType")]
    public class EventTypeController : BaseController<EventType, EventTypeDTO>
    {
        public EventTypeController(BaseService<EventType, EventTypeDTO> service)
            : base(service)
        {
        }
    }
}