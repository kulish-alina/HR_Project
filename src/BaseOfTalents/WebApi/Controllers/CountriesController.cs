using Data.Infrastructure;
using Service.Services;
using Domain.DTO.DTOModels.SetupDTO;
using Domain.Entities.Setup;
using Domain.Repositories;

namespace WebApi.Controllers
{
    public class CountriesController : BoTController<Country, CountryDTO>
    {
        public CountriesController(IControllerService<Country, CountryDTO> service)
            : base(service)
        {
        }
    }
}