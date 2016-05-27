using Domain.Entities.Enum;
using Service.Services;

namespace WebApi.Controllers
{
    public class SourcesController : EnumController<Source>
    {
        public SourcesController(IEnumService<Source> service) : base(service)
        {
        }
    }
}