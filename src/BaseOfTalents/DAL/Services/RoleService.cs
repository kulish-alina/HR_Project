using DAL.DTO;
using DAL.Infrastructure;
using Domain.Entities.Enum.Setup;

namespace DAL.Services
{
    public class RoleService : BaseService<Role, RoleDTO>
    {
        public RoleService(IUnitOfWork uow) : base(uow, uow.RoleRepo)
        {

        }
    }
}
