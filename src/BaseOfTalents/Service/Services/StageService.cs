using Domain.DTO.DTOModels.SetupDTO;
using Domain.Entities.Setup;
using Domain.Repositories;
using System;

namespace Service.Services
{
    public class StageService : ControllerService<Stage, StageDTO>
    {
        public StageService(IRepository<Stage> repository) : base(repository)
        {
            entityRepository = repository;
        }

        public override object Search(object searchParams)
        {
            throw new NotImplementedException();
        }
    }
}
