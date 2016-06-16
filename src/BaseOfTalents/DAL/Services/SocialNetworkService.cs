using BaseOfTalents.DAL.Infrastructure;
using BaseOfTalents.Domain.Entities.Enum.Setup;
using Domain.DTO.DTOModels.SetupDTO;
using System;

namespace DAL.Services
{
    public class SocialNetworkService : BaseService<SocialNetwork, SocialNetworkDTO>
    {
        public SocialNetworkService(IUnitOfWork uow) : base(uow, uow.SocialNetworkRepo)
        {

        }
    }
}
