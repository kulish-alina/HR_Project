using DAL.DTO.SetupDTO;
using DAL.Services;
using Domain.Entities.Enum.Setup;
using System.Web.Http;

namespace WebUI.Controllers
{
    [RoutePrefix("api/Stage")]
    public class StageController : BaseController<Stage, StageDTO>
    {
        public StageController(BaseService<Stage, StageDTO> service)
            : base(service)
        {
        }
    }
}