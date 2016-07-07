using DAL.Services;
using Domain.Entities.Enum;
using System.Web.Http;

namespace WebUI.Controllers
{
    [RoutePrefix("api/TypeOfEmployment")]
    public class TypeOfEmploymentController : BaseEnumController<TypeOfEmployment>
    {
        public TypeOfEmploymentController(BaseEnumService<TypeOfEmployment> service) : base(service)
        {
        }
    }
}