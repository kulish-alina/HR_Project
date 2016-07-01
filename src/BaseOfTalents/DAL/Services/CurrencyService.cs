using DAL.DTO.SetupDTO;
using DAL.Infrastructure;
using Domain.Entities.Enum.Setup;

namespace DAL.Services
{
    public class CurrencyService : BaseService<Currency, CurrencyDTO>
    {
        public CurrencyService(IUnitOfWork uow) : base(uow, uow.CurrencyRepo)
        {

        }
    }
}
