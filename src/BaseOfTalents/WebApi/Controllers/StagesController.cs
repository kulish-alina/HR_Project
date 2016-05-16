using Data.Infrastructure;
using Domain.DTO.DTOModels.SetupDTO;
using Domain.Entities.Setup;
using Domain.Repositories;

namespace WebApi.Controllers
{
    public class StagesController : BoTController<Stage, StageDTO>
    {
        public StagesController(IDataRepositoryFactory repoFatory, IErrorRepository errorRepo)
            : base(repoFatory, errorRepo)
        {
        }
    }
}