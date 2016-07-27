using DAL.DTO.SetupDTO;
using DAL.Services;
using Domain.Entities.Enum.Setup;
using System.Web.Http;

namespace WebUI.Controllers
{
    [RoutePrefix("api/Skill")]
    public class SkillController : BaseController<Skill, SkillDTO>
    {
        public SkillController(BaseService<Skill, SkillDTO> service)
            : base(service)
        {
        }
    }
}