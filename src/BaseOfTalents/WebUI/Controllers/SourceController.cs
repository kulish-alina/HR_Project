using System.Web.Http;
using DAL.DTO.SetupDTO;
using DAL.Services;
using Domain.Entities.Enum.Setup;

namespace WebUI.Controllers
{
    [RoutePrefix("api/Source")]
    public class SourceController : BaseController<Source, SourceDTO>
    {
        public SourceController(SourceService service) : base(service)
        {

        }
    }
}