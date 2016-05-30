using Domain.Entities.Enum;
using Service.Services;

namespace WebApi.Controllers
{
    public class CommentTypesController : EnumController<CommentType>
    {
        public CommentTypesController(IEnumService<CommentType> service) : base(service)
        {
        }
    }
}