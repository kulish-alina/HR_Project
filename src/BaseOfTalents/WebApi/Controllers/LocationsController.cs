using Data.Infrastructure;
using Service.Services;
using Domain.DTO.DTOModels.SetupDTO;
using Domain.Entities.Setup;
using Domain.Repositories;

namespace WebApi.Controllers
{
    public class LocationsController : BoTController<Location, LocationDTO>
    {
        public LocationsController(IControllerService<Location, LocationDTO> service)
            : base(service)
        {
        }
    }
}