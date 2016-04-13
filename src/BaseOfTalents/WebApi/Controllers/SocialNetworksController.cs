using Domain.Entities.Setup;
using Domain.Repositories;
using WebApi.DTO.DTOModels;

namespace WebApi.Controllers
{
    public class SocialNetworksController : BoTController<SocialNetwork, SocialNetworkDTO>
    {
        public SocialNetworksController(ISocialNetworkRepository socialNetworkRepository)
        {
            _repo = socialNetworkRepository;
        }
    }
}