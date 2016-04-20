using Data.EFData.Design;
using Domain.Entities.Setup;
using Domain.Repositories;

namespace WebApi.Controllers
{
    public class SocialNetworksController : BoTController<SocialNetwork, SocialNetwork>
    {
        public SocialNetworksController(IRepositoryFacade facade) : base(facade)
        {
            _currentRepo = _repoFacade.SocialNetworkRepository;
        }
    }
}