using Domain.Entities.Enum;
using Service.Services;

namespace WebApi.Controllers
{
    public class LanguageLevelsController : EnumController<LanguageLevel>
    {
        public LanguageLevelsController(IEnumService<LanguageLevel> service) : base(service)
        {
        }
    }
}