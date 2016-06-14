using BaseOfTalents.Domain.Entities.Enum.Setup;
using DAL.Services;
using Domain.DTO.DTOModels.SetupDTO;
using System.Web.Http;

namespace WebApi.Controllers
{
    [RoutePrefix("api/tag")]
    public class TagController : BaseController<Tag, TagDTO>
    {
        public TagController(BaseService<Tag, TagDTO> service)
            : base(service)
        {
        }
    }
}