using BaseOfTalents.DAL.Infrastructure;
using DAL.DTO.SetupDTO;
using Domain.Entities.Enum.Setup;

namespace DAL.Services
{
    public class SourceService : BaseService<Source, SourceDTO>
    {
        public SourceService(IUnitOfWork uow) : base(uow, uow.SourceRepo)
        {

        }
    }
}
