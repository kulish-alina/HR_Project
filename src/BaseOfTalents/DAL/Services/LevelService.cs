using DAL.DTO.SetupDTO;
using DAL.Infrastructure;
using Domain.Entities.Enum.Setup;

namespace DAL.Services
{
    public class LevelService : BaseService<Level, LevelDTO>
    {
        public LevelService(IUnitOfWork uow) : base(uow, uow.LevelRepo)
        {

        }
    }
}
