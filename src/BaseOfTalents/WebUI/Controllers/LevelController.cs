using DAL.DTO.SetupDTO;
using DAL.Services;
using Domain.Entities.Enum.Setup;
using System.Web.Http;

namespace WebUI.Controllers
{
    [RoutePrefix("api/Level")]
    public class LevelController : BaseController<Level, LevelDTO>
    {
        public LevelController(BaseService<Level, LevelDTO> service)
            : base(service)
        {
        }
    }
}