using Domain.DTO.DTOModels.SetupDTO;
using Domain.Entities.Enum.Setup;
using Domain.Entities.Setup;
using Domain.Repositories;
using System;

namespace Service.Services
{
    public class IndustryService : ControllerService<Industry, IndustryDTO>
    {
        public IndustryService(IRepository<Industry> repository) : base(repository)
        {
            entityRepository = repository;
        }

        public override object Search(object searchParams)
        {
            throw new NotImplementedException();
        }
    }
}
