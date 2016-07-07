using DAL.DTO;
using DAL.Infrastructure;
using Domain.Entities.Enum.Setup;

namespace DAL.Services
{
    public class PermissionService : BaseService<Permission, PermissionDTO>
    {
        public PermissionService(IUnitOfWork uow) : base(uow, uow.PermissionRepo)
        {

        }
    }
}
