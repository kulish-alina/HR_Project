using Data.Infrastructure;
using Service.Services;
using Domain.DTO.DTOModels.SetupDTO;
using Domain.Entities.Enum.Setup;
using Domain.Repositories;

namespace WebApi.Controllers
{
    public class IndustriesController : BoTController<Industry, IndustryDTO>
    {
        public IndustriesController(IControllerService<Industry, IndustryDTO> service)
            : base(service)
        {
        }
    }
}