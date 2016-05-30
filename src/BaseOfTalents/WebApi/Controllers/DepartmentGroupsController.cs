using Data.Infrastructure;
using Service.Services;
using Domain.DTO.DTOModels.SetupDTO;
using Domain.Entities.Setup;
using Domain.Repositories;

namespace WebApi.Controllers
{
    public class DepartmentGroupsController : BoTController<DepartmentGroup, DepartmentGroupDTO>
    {
        public DepartmentGroupsController(IControllerService<DepartmentGroup, DepartmentGroupDTO> service)
            : base(service)
        {
        }
    }
}