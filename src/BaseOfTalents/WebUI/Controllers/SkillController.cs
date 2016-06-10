using BaseOfTalents.Domain.Entities.Enum.Setup;
using DAL.Services;
using Domain.DTO.DTOModels.SetupDTO;
using System.Web.Http;

namespace WebApi.Controllers
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