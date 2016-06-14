using BaseOfTalents.Domain.Entities.Enum;
using DAL.Services;
using Domain.Entities.Enum;
using System.Web.Http;

namespace WebApi.Controllers
{
    [RoutePrefix("api/CommentType")]
    public class CommentTypeController : BaseEnumController<CommentType>
    {
        public CommentTypeController(BaseEnumService<CommentType> service) : base(service)
        {
        }
    }
}