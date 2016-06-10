using BaseOfTalents.Domain.Entities.Enum.Setup;
using DAL.Services;
using Domain.DTO.DTOModels.SetupDTO;
using System.Web.Http;

namespace WebApi.Controllers
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