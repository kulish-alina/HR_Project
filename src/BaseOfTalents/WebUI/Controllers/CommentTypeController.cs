using DAL.Services;
using Entities.Enum;
using System.Web.Http;

namespace WebUI.Controllers
{
    [RoutePrefix("api/CommentType")]
    public class CommentTypeController : BaseEnumController<CommentType>
    {
        public CommentTypeController(BaseEnumService<CommentType> service) : base(service)
        {
        }
    }
}