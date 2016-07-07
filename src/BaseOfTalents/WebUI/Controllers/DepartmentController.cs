using DAL.DTO.SetupDTO;
using DAL.Services;
using Domain.Entities.Enum.Setup;
using System.Web.Http;

namespace WebUI.Controllers
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