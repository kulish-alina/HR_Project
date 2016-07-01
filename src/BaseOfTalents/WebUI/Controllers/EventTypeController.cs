using DAL.DTO.SetupDTO;
using DAL.Services;
using Domain.Entities.Enum.Setup;
using System.Web.Http;

namespace WebUI.Controllers
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