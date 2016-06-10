using BaseOfTalents.DAL.Infrastructure;
using BaseOfTalents.Domain.Entities.Enum.Setup;
using Domain.DTO.DTOModels.SetupDTO;

namespace DAL.Services
{
    public class LocationService : BaseService<Location, LocationDTO>
    {
        public LocationService(IUnitOfWork uow) : base(uow, uow.LocationRepo)
        {

        }
    }
}
