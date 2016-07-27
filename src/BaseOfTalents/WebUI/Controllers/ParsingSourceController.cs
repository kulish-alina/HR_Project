using DAL.Services;
using Domain.Entities.Enum;
using System.Web.Http;

namespace WebUI.Controllers
{
    [RoutePrefix("api/ParsingSource")]
    public class ParsingSourceController : BaseEnumController<ParsingSource>
    {
        public ParsingSourceController(BaseEnumService<ParsingSource> service) : base(service)
        {
        }
    }
}