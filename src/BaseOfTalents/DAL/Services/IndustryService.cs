using BaseOfTalents.DAL.Infrastructure;
using BaseOfTalents.Domain.Entities.Enum.Setup;
using Domain.DTO.DTOModels.SetupDTO;
using Domain.Entities.Enum.Setup;
using System;

namespace DAL.Services
{
    public class IndustryService : BaseService<Industry, IndustryDTO>
    {
        public IndustryService(IUnitOfWork uow) : base(uow, uow.IndustryRepo)
        {

        }
    }
}
