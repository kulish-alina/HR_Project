using BaseOfTalents.Domain.Entities.Enum.Setup;
using DAL.Services;
using Domain.DTO.DTOModels;
using System.Web.Http;

namespace WebApi.Controllers
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