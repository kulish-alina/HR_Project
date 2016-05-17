using Data.Infrastructure;
using Domain.DTO.DTOModels.SetupDTO;
using Domain.Entities.Setup;
using Domain.Repositories;

namespace WebApi.Controllers
{
    public class SkillsController : BoTController<Skill, SkillDTO>
    {
        public SkillsController(IDataRepositoryFactory repoFatory, IErrorRepository errorRepo)
            : base(repoFatory, errorRepo)
        {
        }
    }
}