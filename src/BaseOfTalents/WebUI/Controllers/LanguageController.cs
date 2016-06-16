using BaseOfTalents.Domain.Entities.Enum.Setup;
using DAL.Services;
using Domain.DTO.DTOModels.SetupDTO;
using System.Web.Http;

namespace WebApi.Controllers
{
    [RoutePrefix("api/Language")]
    public class LanguageController : BaseController<Language, LanguageDTO>
    {
        public LanguageController(BaseService<Language, LanguageDTO> service)
            : base(service)
        {
        }
    }
}