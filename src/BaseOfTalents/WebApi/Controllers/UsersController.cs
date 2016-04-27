using Data.Infrastructure;
using Domain.DTO.DTOModels;
using Domain.Entities;
using Domain.Repositories;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class UsersController : BoTController<User, UserDTO>
    {
        public UsersController(IDataRepositoryFactory repoFatory, IUnitOfWork unitOfWork, IErrorRepository errorRepo)
            : base (repoFatory, unitOfWork, errorRepo)
        {

        }
    }
}
