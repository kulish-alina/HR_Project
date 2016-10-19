using System.Web.Http;
using DAL.DTO;
using DAL.Services;
using Domain.Entities;

namespace WebUI.Controllers
{
    [RoutePrefix("api/mail")]
    public class MailController : BaseController<MailContent, MailDTO>
    {
        public MailController(BaseService<MailContent, MailDTO> service) : base(service)
        {
        }
    }
}
