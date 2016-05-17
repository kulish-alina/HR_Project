using Data.Infrastructure;
using Domain.DTO.DTOModels;
using Domain.Entities;
using Domain.Repositories;

namespace WebApi.Controllers
{
    public class FilesController : BoTController<File, FileDTO>
    {
        public FilesController(IDataRepositoryFactory repoFatory, IErrorRepository errorRepo)
            : base(repoFatory, errorRepo)
        {
        }
    }
}