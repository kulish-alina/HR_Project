using DAL.DTO.SetupDTO;
using DAL.Infrastructure;
using Domain.Entities.Enum.Setup;

namespace DAL.Services
{
    public class IndustryService : BaseService<Industry, IndustryDTO>
    {
        public IndustryService(IUnitOfWork uow) : base(uow, uow.IndustryRepo)
        {

        }
    }
}
