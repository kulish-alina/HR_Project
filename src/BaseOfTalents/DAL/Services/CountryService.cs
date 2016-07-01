using DAL.DTO.SetupDTO;
using DAL.Infrastructure;
using Domain.Entities.Enum.Setup;

namespace DAL.Services
{
    public class CountryService : BaseService<Country, CountryDTO>
    {
        public CountryService(IUnitOfWork uow) : base(uow, uow.CountryRepo)
        {

        }
    }
}
