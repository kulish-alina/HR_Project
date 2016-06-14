using BaseOfTalents.Domain.Entities.Enum;
using DAL.Services;
using System.Web.Http;

namespace WebApi.Controllers
{
    [RoutePrefix("api/AccessRight")]
    public class AccessRightController : BaseEnumController<AccessRights>
    {
        public AccessRightController(BaseEnumService<AccessRights> service) : base(service)
        {

        }
    }
}