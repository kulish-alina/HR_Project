using Domain.Entities.Enum;
using Service.Services;

namespace WebApi.Controllers
{
    public class TypesOfEmploymentController : EnumController<TypeOfEmployment>
    {
        public TypesOfEmploymentController(IEnumService<TypeOfEmployment> service) : base(service)
        {
        }
    }
}