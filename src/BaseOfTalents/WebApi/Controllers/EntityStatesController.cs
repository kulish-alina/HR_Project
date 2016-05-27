using Domain.Entities.Enum;
using Service.Services;

namespace WebApi.Controllers
{
    public class EntityStatesController : EnumController<EntityState>
    {
        public EntityStatesController(IEnumService<EntityState> service) : base(service)
        {
        }
    }
}