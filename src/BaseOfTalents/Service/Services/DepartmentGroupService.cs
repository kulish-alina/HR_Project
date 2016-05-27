using Domain.DTO.DTOModels.SetupDTO;
using Domain.Entities.Setup;
using Domain.Repositories;
using System;

namespace Service.Services
{
    public class DepartmentGroupService : ControllerService<DepartmentGroup, DepartmentGroupDTO>
    {
        public DepartmentGroupService(IRepository<DepartmentGroup> repository) : base(repository)
        {
            entityRepository = repository;
        }

        public override object Search(object searchParams)
        {
            throw new NotImplementedException();
        }
    }
}
