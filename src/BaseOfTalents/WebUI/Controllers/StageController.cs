using BaseOfTalents.Domain.Entities.Enum.Setup;
using DAL.Services;
using Domain.DTO.DTOModels;
using Domain.DTO.DTOModels.SetupDTO;
using System.Web.Http;

namespace WebApi.Controllers
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