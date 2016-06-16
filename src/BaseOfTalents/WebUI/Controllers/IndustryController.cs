using BaseOfTalents.Domain.Entities.Enum.Setup;
using DAL.Services;
using Domain.DTO.DTOModels.SetupDTO;
using System.Web.Http;

namespace WebApi.Controllers
{
    [RoutePrefix("api/Industry")]
    public class IndustryController : BaseController<Industry, IndustryDTO>
    {
        public IndustryController(BaseService<Industry, IndustryDTO> service)
            : base(service)
        {
        }
    }
}