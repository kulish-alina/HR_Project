using DAL.DTO.SetupDTO;
using DAL.Infrastructure;
using Domain.Entities.Enum.Setup;

namespace DAL.Services
{
    public class SocialNetworkService : BaseService<SocialNetwork, SocialNetworkDTO>
    {
        public SocialNetworkService(IUnitOfWork uow) : base(uow, uow.SocialNetworkRepo)
        {

        }
    }
}
