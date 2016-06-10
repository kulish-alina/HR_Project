using BaseOfTalents.DAL.Infrastructure;
using BaseOfTalents.Domain.Entities.Enum.Setup;
using Domain.DTO.DTOModels;
using Domain.DTO.DTOModels.SetupDTO;

namespace DAL.Services
{
    public class PermissionService : BaseService<Permission, PermissionDTO>
    {
        public PermissionService(IUnitOfWork uow) : base(uow, uow.PermissionRepo)
        {

        }
    }
}
