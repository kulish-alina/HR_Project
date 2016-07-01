using DAL.DTO.SetupDTO;
using DAL.Services;
using Domain.Entities.Enum.Setup;
using System.Web.Http;

namespace WebUI.Controllers
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