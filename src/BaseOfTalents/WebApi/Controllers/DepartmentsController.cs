using Data.Infrastructure;
using Service.Services;
using Domain.DTO.DTOModels.SetupDTO;
using Domain.Entities.Setup;
using Domain.Repositories;

namespace WebApi.Controllers
{
    public class DepartmentsController : BoTController<Department, DepartmentDTO>
    {
        public DepartmentsController(IControllerService<Department, DepartmentDTO> service)
            : base(service)
        {
        }
    }
}