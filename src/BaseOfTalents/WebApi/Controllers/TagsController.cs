using Data.Infrastructure;
using Domain.DTO.DTOModels.SetupDTO;
using Domain.Entities.Enum.Setup;
using Domain.Repositories;

namespace WebApi.Controllers
{
    public class TagsController : BoTController<Tag, TagDTO>
    {
        public TagsController(IDataRepositoryFactory repoFatory, IErrorRepository errorRepo)
            : base(repoFatory, errorRepo)
        {
        }
    }
}