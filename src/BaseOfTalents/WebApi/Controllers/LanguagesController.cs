using Data.Infrastructure;
using Service.Services;
using Domain.DTO.DTOModels.SetupDTO;
using Domain.Entities.Setup;
using Domain.Repositories;

namespace WebApi.Controllers
{
    public class LanguagesController : BoTController<Language, LanguageDTO>
    {
        public LanguagesController(IControllerService<Language, LanguageDTO> service)
            : base(service)
        {
        }
    }
}