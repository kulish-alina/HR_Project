using Domain.Entities.Setup;
using Domain.Repositories;

namespace WebApi.Controllers
{
    public class ExperiencesController : BoTController<Experience, Experience>
    {
        public ExperiencesController(IExperienceRepository experienceRepository)
        {
            _repo = experienceRepository;
        }
    }
}