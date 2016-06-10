using BaseOfTalents.DAL.Infrastructure;
using BaseOfTalents.Domain.Entities.Enum.Setup;
using DAL.Services;
using Domain.DTO.DTOModels.SetupDTO;
using System;

namespace DAL.Services
{
    public class DepartmentGroupService : BaseService<DepartmentGroup, DepartmentGroupDTO>
    {
        public DepartmentGroupService(IUnitOfWork uow) : base(uow, uow.DepartmentGroupRepo)
        {

        }
    }
}
