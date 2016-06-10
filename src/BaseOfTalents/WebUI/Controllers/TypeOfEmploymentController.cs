using BaseOfTalents.Domain.Entities.Enum;
using DAL.Services;
using System.Web.Http;

namespace WebApi.Controllers
{
    [RoutePrefix("api/TypeOfEmployment")]
    public class TypeOfEmploymentController : BaseEnumController<TypeOfEmployment>
    {
        public TypeOfEmploymentController(BaseEnumService<TypeOfEmployment> service) : base(service)
        {
        }
    }
}