using DAL.Services;
using Domain.Entities.Enum;
using System.Web.Http;

namespace WebUI.Controllers
{
    [RoutePrefix("api/AccessRight")]
    public class AccessRightController : BaseEnumController<AccessRight>
    {
        public AccessRightController(BaseEnumService<AccessRight> service) : base(service)
        {

        }
    }
}