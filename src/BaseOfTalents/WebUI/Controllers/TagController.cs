using DAL.DTO.SetupDTO;
using DAL.Services;
using Domain.Entities.Enum.Setup;
using System.Web.Http;

namespace WebUI.Controllers
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