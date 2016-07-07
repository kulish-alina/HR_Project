using DAL.DTO.SetupDTO;
using DAL.Services;
using Domain.Entities.Enum.Setup;
using System.Web.Http;

namespace WebUI.Controllers
{
    [RoutePrefix("api/city")]
    public class CityController : BaseController<City, CityDTO>
    {
        public CityController(BaseService<City, CityDTO> service)
            : base(service)
        {
        }
    }
}