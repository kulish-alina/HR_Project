using Data.Infrastructure;
using Service.Services;
using Domain.DTO.DTOModels.SetupDTO;
using Domain.Entities.Setup;
using Domain.Repositories;

namespace WebApi.Controllers
{
    public class StagesController : BoTController<Stage, StageDTO>
    {
        public StagesController(IControllerService<Stage, StageDTO> service)
            : base(service)
        {
        }
    }
}