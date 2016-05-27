using Data.Infrastructure;
using Service.Services;
using Domain.DTO.DTOModels.SetupDTO;
using Domain.Entities.Setup;
using Domain.Repositories;

namespace WebApi.Controllers
{
    public class SkillsController : BoTController<Skill, SkillDTO>
    {
        public SkillsController(IControllerService<Skill, SkillDTO> service)
            : base(service)
        {
        }
    }
}