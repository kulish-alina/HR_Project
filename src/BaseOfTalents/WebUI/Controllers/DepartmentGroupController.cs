using DAL.DTO.SetupDTO;
using DAL.Services;
using Domain.Entities.Enum.Setup;
using System.Web.Http;

namespace WebUI.Controllers
{
    [RoutePrefix("api/DepartmentGroup")]
    public class DepartmentGroupController : BaseController<DepartmentGroup, DepartmentGroupDTO>
    {
        public DepartmentGroupController(BaseService<DepartmentGroup, DepartmentGroupDTO> service)
            : base(service)
        {
        }
    }
}