using BaseOfTalents.Domain.Entities.Enum.Setup;
using DAL.Services;
using Domain.DTO.DTOModels.SetupDTO;
using System.Web.Http;

namespace WebApi.Controllers
{
    [RoutePrefix("api/Location")]
    public class LocationController : BaseController<Location, LocationDTO>
    {
        public LocationController(BaseService<Location, LocationDTO> service)
            : base(service)
        {
        }
    }
}