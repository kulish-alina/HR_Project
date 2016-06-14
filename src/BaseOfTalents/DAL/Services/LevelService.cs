using BaseOfTalents.DAL.Infrastructure;
using BaseOfTalents.Domain.Entities.Enum.Setup;
using Domain.DTO.DTOModels.SetupDTO;

namespace DAL.Services
{
    public class LevelService : BaseService<Level, LevelDTO>
    {
        public LevelService(IUnitOfWork uow) : base(uow, uow.LevelRepo)
        {

        }
    }
}
