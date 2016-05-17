using Data.Infrastructure;
using Domain.DTO.DTOModels;
using Domain.Entities.Setup;
using Domain.Repositories;

namespace WebApi.Controllers
{
    public class RolesController : BoTController<Role, RoleDTO>
    {
        public RolesController(IDataRepositoryFactory repoFatory, IErrorRepository errorRepo)
            : base(repoFatory, errorRepo)
        {
        }
    }
}