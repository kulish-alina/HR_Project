using DAL.DTO.SetupDTO;
using DAL.Infrastructure;
using Domain.Entities.Enum.Setup;

namespace DAL.Services
{
    public class SkillService : BaseService<Skill, SkillDTO>
    {
        public SkillService(IUnitOfWork uow) : base(uow, uow.SkillRepo)
        {

        }
    }
}
