using DAL.DTO.SetupDTO;
using DAL.Infrastructure;
using Domain.Entities.Enum.Setup;

namespace DAL.Services
{
    public class DepartmentService : BaseService<Department, DepartmentDTO>
    {
        public DepartmentService(IUnitOfWork uow) : base(uow, uow.DepartmentRepo)
        {

        }
    }
}
