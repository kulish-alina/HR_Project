using BaseOfTalents.Domain.Entities.Enum.Setup;
using DAL.Services;
using Domain.DTO.DTOModels;
using Domain.DTO.DTOModels.SetupDTO;
using System.Web.Http;

namespace WebApi.Controllers
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