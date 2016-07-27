using DAL.DTO.SetupDTO;
using DAL.Services;
using Domain.Entities.Enum.Setup;
using System.Web.Http;

namespace WebUI.Controllers
{
    [RoutePrefix("api/Industry")]
    public class IndustryController : BaseController<Industry, IndustryDTO>
    {
        public IndustryController(BaseService<Industry, IndustryDTO> service)
            : base(service)
        {
        }
    }
}