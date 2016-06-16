using BaseOfTalents.DAL.Infrastructure;
using BaseOfTalents.Domain.Entities.Enum.Setup;
using Domain.DTO.DTOModels;
using System;

namespace DAL.Services
{ 
    public class RoleService : BaseService<Role, RoleDTO>
    {
        public RoleService(IUnitOfWork uow) : base(uow, uow.RoleRepo)
        {

        }
    }
}
