using DAL.DTO;
using DAL.Services;
using Domain.Entities.Enum.Setup;
using System.Web.Http;

namespace WebUI.Controllers
{
    [RoutePrefix("api/Role")]
    public class RoleController : BaseController<Role, RoleDTO>
    {
        public RoleController(BaseService<Role, RoleDTO> service)
            : base(service)
        {
        }
    }
}