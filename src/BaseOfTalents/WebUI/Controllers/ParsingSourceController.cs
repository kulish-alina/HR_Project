using BaseOfTalents.Domain.Entities.Enum;
using DAL.Services;
using System.Web.Http;

namespace WebApi.Controllers
{
    [RoutePrefix("api/ParsingSource")]
    public class ParsingSourceController : BaseEnumController<ParsingSource>
    {
        public ParsingSourceController(BaseEnumService<ParsingSource> service) : base(service)
        {
        }
    }
}