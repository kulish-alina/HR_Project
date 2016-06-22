using BaseOfTalents.Domain.Entities.Enum;
using DAL.Services;
using System.Web.Http;

namespace WebApi.Controllers
{
    [RoutePrefix("api/AccessRight")]
    public class AccessRightController : BaseEnumController<AccessRight>
    {
        public AccessRightController(BaseEnumService<AccessRight> service) : base(service)
        {

        }
    }
}