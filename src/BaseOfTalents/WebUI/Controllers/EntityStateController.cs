using BaseOfTalents.Domain.Entities.Enum;
using DAL.Services;
using System.Web.Http;

namespace WebApi.Controllers
{
    [RoutePrefix("api/EntityState")]
    public class EntityStateController : BaseEnumController<EntityState>
    {
        public EntityStateController(BaseEnumService<EntityState> service) : base(service)
        {
        }
    }
}