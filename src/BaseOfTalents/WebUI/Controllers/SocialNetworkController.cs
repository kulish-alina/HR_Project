using DAL.DTO.SetupDTO;
using DAL.Services;
using Domain.Entities.Enum.Setup;
using System.Web.Http;

namespace WebUI.Controllers
{
    [RoutePrefix("api/SocialNetwork")]
    public class SocialNetworkController : BaseController<SocialNetwork, SocialNetworkDTO>
    {
        public SocialNetworkController(BaseService<SocialNetwork, SocialNetworkDTO> service)
            : base(service)
        {
        }
    }
}