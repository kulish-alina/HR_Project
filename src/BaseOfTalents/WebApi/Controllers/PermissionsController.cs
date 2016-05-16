using Data.Infrastructure;
using Domain.DTO.DTOModels;
using Domain.Entities.Setup;
using Domain.Repositories;

namespace WebApi.Controllers
{
    public class PermissionsController : BoTController<Permission, PermissionDTO>
    {
        public PermissionsController(IDataRepositoryFactory repoFatory, IErrorRepository errorRepo)
            : base(repoFatory, errorRepo)
        {
        }

        public PermissionsController()
        {
        }
    }
}