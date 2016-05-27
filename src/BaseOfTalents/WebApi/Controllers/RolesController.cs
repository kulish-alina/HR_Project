using Data.Infrastructure;
using Service.Services;
using Domain.DTO.DTOModels;
using Domain.Entities.Setup;
using Domain.Repositories;

namespace WebApi.Controllers
{
    public class RolesController : BoTController<Role, RoleDTO>
    {
        public RolesController(IControllerService<Role, RoleDTO> service)
            : base(service)
        {
        }
    }
}