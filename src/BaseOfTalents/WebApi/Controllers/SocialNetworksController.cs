using Data.Infrastructure;
using Service.Services;
using Domain.DTO.DTOModels.SetupDTO;
using Domain.Entities.Setup;
using Domain.Repositories;

namespace WebApi.Controllers
{
    public class SocialNetworksController : BoTController<SocialNetwork, SocialNetworkDTO>
    {
        public SocialNetworksController(IControllerService<SocialNetwork, SocialNetworkDTO> service)
            : base(service)
        {
        }
    }
}