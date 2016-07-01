using DAL.DTO.SetupDTO;
using DAL.Infrastructure;
using Domain.Entities.Enum.Setup;

namespace DAL.Services
{
    public class DepartmentGroupService : BaseService<DepartmentGroup, DepartmentGroupDTO>
    {
        public DepartmentGroupService(IUnitOfWork uow) : base(uow, uow.DepartmentGroupRepo)
        {

        }
    }
}
