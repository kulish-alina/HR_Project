using Data.Infrastructure;
using Domain.DTO.DTOModels.SetupDTO;
using Domain.Entities.Enum.Setup;
using Domain.Repositories;

namespace WebApi.Controllers
{
    public class IndustriesController : BoTController<Industry, IndustryDTO>
    {
        public IndustriesController(IDataRepositoryFactory repoFatory, IErrorRepository errorRepo)
            : base(repoFatory, errorRepo)
        {
        }
    }
}