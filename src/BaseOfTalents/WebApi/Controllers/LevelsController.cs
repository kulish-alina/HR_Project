using Data.Infrastructure;
using Service.Services;
using Domain.DTO.DTOModels.SetupDTO;
using Domain.Entities.Enum.Setup;
using Domain.Repositories;

namespace WebApi.Controllers
{
    public class LevelsController : BoTController<Level, LevelDTO>
    {
        public LevelsController(IControllerService<Level, LevelDTO> service)
            : base(service)
        {
        }
    }
}