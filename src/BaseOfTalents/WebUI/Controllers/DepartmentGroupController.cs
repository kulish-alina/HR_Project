using BaseOfTalents.Domain.Entities.Enum.Setup;
using DAL.Services;
using Domain.DTO.DTOModels.SetupDTO;
using System.Web.Http;

namespace WebApi.Controllers
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