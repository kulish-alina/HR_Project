using DAL.Services;
using Domain.Entities.Enum;
using System.Web.Http;

namespace WebUI.Controllers
{
    [RoutePrefix("api/EntityState")]
    public class EntityStateController : BaseEnumController<EntityState>
    {
        public EntityStateController(BaseEnumService<EntityState> service) : base(service)
        {
        }
    }
}