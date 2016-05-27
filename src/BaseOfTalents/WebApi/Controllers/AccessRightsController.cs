using Domain.Entities.Enum;
using Service.Services;

namespace WebApi.Controllers
{
    public class AccessRightsController : EnumController<AccessRights>
    {
        public AccessRightsController(IEnumService<AccessRights> service) : base(service)
        {

        }
    }
}