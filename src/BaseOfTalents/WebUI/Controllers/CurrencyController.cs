using DAL.DTO.SetupDTO;
using DAL.Services;
using Domain.Entities.Enum.Setup;
using System.Web.Http;

namespace WebUI.Controllers
{
    [RoutePrefix("api/currency")]
    public class CurrencyController : BaseController<Currency, CurrencyDTO>
    {
        public CurrencyController(BaseService<Currency, CurrencyDTO> service)
            : base(service)
        {
        }
    }
}
