using DAL.DTO.SetupDTO;
using DAL.Infrastructure;
using Domain.Entities.Enum.Setup;

namespace DAL.Services
{
    public class CityService : BaseService<City, CityDTO>
    {
        public CityService(IUnitOfWork uow) : base(uow, uow.CityRepo)
        {

        }
    }
}
