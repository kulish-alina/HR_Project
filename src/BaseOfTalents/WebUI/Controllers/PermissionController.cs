using BaseOfTalents.Domain.Entities.Enum.Setup;
using DAL.Services;
using Domain.DTO.DTOModels;
using System.Web.Http;

namespace WebApi.Controllers
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