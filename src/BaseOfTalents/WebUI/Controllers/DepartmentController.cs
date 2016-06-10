using BaseOfTalents.Domain.Entities.Enum.Setup;
using DAL.Services;
using Domain.DTO.DTOModels.SetupDTO;
using System.Web.Http;

namespace WebApi.Controllers
{
    [RoutePrefix("api/Department")]
    public class DepartmentController : BaseController<Department, DepartmentDTO>
    {
        public DepartmentController(BaseService<Department, DepartmentDTO> service)
            : base(service)
        {
        }
    }
}