using Domain.DTO.DTOModels;
using Domain.DTO.DTOModels.SetupDTO;
using Domain.Entities.Setup;
using Domain.Repositories;
using System;

namespace Service.Services
{
    public class PermissionService : ControllerService<Permission, PermissionDTO>
    {
        public PermissionService(IRepository<Permission> repository) : base(repository)
        {
            entityRepository = repository;
        }

        public override object Search(object searchParams)
        {
            throw new NotImplementedException();
        }
    }
}
