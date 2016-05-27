using Data.Infrastructure;
using Service.Services;
using Domain.DTO.DTOModels.SetupDTO;
using Domain.Entities.Enum.Setup;
using Domain.Repositories;

namespace WebApi.Controllers
{
    public class TagsController : BoTController<Tag, TagDTO>
    {
        public TagsController(IControllerService<Tag, TagDTO> service)
            : base(service)
        {
        }
    }
}