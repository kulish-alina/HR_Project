using Domain.Entities.Setup;
using Domain.Repositories;

namespace WebApi.Controllers
{
    public class RoomsController : BoTController<Room, Room>
    {
        public RoomsController(IRoomRepository roomRepository)
        {
            _repo = roomRepository;
        }
    }
}