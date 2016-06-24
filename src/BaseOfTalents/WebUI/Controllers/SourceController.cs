using DAL.DTO.SetupDTO;
using DAL.Services;
using Domain.Entities.Enum.Setup;
using System.Web.Http;
using WebApi.Controllers;

namespace BaseOfTalents.WebUI.Controllers
{
    [RoutePrefix("api/Source")]
    public class SourceController : BaseController<Source, SourceDTO>
    {
        public SourceController(SourceService service) : base(service)
        {

        }

        public SourceController()
        {

        }
    }
}