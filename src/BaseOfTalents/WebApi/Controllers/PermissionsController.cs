using Data.Infrastructure;
using Service.Services;
using Domain.DTO.DTOModels;
using Domain.Entities.Setup;
using Domain.Repositories;

namespace WebApi.Controllers
{
    public class PermissionsController : BoTController<Permission, PermissionDTO>
    {
        public PermissionsController(IControllerService<Permission, PermissionDTO> service)
            : base(service)
        {
        }
    }
}