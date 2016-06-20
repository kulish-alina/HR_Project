using BaseOfTalents.DAL.Infrastructure;
using BaseOfTalents.Domain.Entities.Enum.Setup;
using Domain.DTO.DTOModels.SetupDTO;

namespace DAL.Services
{
    public class CityService : BaseService<City, CityDTO>
    {
        public CityService(IUnitOfWork uow) : base(uow, uow.CityRepo)
        {

        }
    }
}
