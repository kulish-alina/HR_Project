using DAL.DTO.SetupDTO;
using DAL.Services;
using Domain.Entities.Enum.Setup;
using System.Web.Http;

namespace WebUI.Controllers
{
    [RoutePrefix("api/Country")]
    public class CountryController : BaseController<Country, CountryDTO>
    {
        public CountryController(BaseService<Country, CountryDTO> service)
            : base(service)
        {
        }
    }
}