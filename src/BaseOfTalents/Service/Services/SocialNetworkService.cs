using Domain.DTO.DTOModels.SetupDTO;
using Domain.Entities.Setup;
using Domain.Repositories;
using System;

namespace Service.Services
{
    public class SocialNetworkService : ControllerService<SocialNetwork, SocialNetworkDTO>
    {
        public SocialNetworkService(IRepository<SocialNetwork> repository) : base(repository)
        {
            entityRepository = repository;
        }

        public override object Search(object searchParams)
        {
            throw new NotImplementedException();
        }
    }
}
