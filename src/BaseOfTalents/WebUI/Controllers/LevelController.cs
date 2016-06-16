using BaseOfTalents.Domain.Entities.Enum.Setup;
using DAL.Services;
using Domain.DTO.DTOModels.SetupDTO;
using System.Web.Http;

namespace WebApi.Controllers
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