using BaseOfTalents.DAL;
using BaseOfTalents.DAL.Infrastructure;
using BaseOfTalents.Domain.Entities.Enum.Setup;
using DAL.Services;
using Domain.DTO.DTOModels.SetupDTO;

namespace DAL.Services
{
    public class CountryService : BaseService<Country, CountryDTO>
    {
        public CountryService(IUnitOfWork uow) : base(uow, uow.CountryRepo)
        {

        }
    }
}
