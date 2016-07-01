using DAL.DTO.SetupDTO;
using DAL.Infrastructure;
using Domain.Entities.Enum.Setup;

namespace DAL.Services
{
    public class StageService : BaseService<Stage, StageDTO>
    {
        public StageService(IUnitOfWork uow) : base(uow, uow.StageRepo)
        {

        }
    }
}
