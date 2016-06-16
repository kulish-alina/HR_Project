using BaseOfTalents.Domain.Entities.Enum;
using DAL.Services;
using System.Web.Http;

namespace WebApi.Controllers
{
    [RoutePrefix("api/Source")]
    public class SourceController : BaseEnumController<Source>
    {
        public SourceController(BaseEnumService<Source> service) : base(service)
        {
        }
    }
}