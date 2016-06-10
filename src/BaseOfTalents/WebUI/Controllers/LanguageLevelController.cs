using BaseOfTalents.Domain.Entities.Enum;
using DAL.Services;
using System.Web.Http;

namespace WebApi.Controllers
{
    [RoutePrefix("api/LanguageLevel")]
    public class LanguageLevelController : BaseEnumController<LanguageLevel>
    {
        public LanguageLevelController(BaseEnumService<LanguageLevel> service) : base(service)
        {
        }
    }
}