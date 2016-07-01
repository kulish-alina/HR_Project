using DAL.DTO;
using DAL.Services;
using Domain.Entities.Enum.Setup;
using System.Web.Http;

namespace WebUI.Controllers
{
    [RoutePrefix("api/Permission")]
    public class PermissionController : BaseController<Permission, PermissionDTO>
    {
        public PermissionController(BaseService<Permission, PermissionDTO> service)
            : base(service)
        {
        }
    }
}