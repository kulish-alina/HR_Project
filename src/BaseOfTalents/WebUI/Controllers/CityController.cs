using BaseOfTalents.Domain.Entities.Enum.Setup;
using DAL.Services;
using Domain.DTO.DTOModels.SetupDTO;
using System.Web.Http;

namespace WebApi.Controllers
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