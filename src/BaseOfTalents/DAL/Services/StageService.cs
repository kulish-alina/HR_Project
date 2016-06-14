using BaseOfTalents.DAL.Infrastructure;
using BaseOfTalents.Domain.Entities.Enum.Setup;
using Domain.DTO.DTOModels.SetupDTO;
using System;

namespace DAL.Services
{
    public class StageService : BaseService<Stage, StageDTO>
    {
        public StageService(IUnitOfWork uow) : base(uow, uow.StageRepo)
        {

        }
    }
}
