using BaseOfTalents.DAL.Infrastructure;
using BaseOfTalents.Domain.Entities.Enum.Setup;
using DAL.Services;
using Domain.DTO.DTOModels.SetupDTO;
using System;

namespace DAL.Services
{
    public class DepartmentService : BaseService<Department, DepartmentDTO>
    {
        public DepartmentService(IUnitOfWork uow) : base(uow, uow.DepartmentRepo)
        {

        }
    }
}
