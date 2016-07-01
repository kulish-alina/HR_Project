using DAL.Services;
using Domain.Entities.Enum;
using System.Web.Http;

namespace WebUI.Controllers
{
    [RoutePrefix("api/LanguageLevel")]
    public class LanguageLevelController : BaseEnumController<LanguageLevel>
    {
        public LanguageLevelController(BaseEnumService<LanguageLevel> service) : base(service)
        {
        }
    }
}