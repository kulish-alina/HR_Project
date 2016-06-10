using BaseOfTalents.DAL.Infrastructure;
using BaseOfTalents.Domain.Entities.Enum.Setup;
using Domain.DTO.DTOModels.SetupDTO;
using System;

namespace DAL.Services
{
    public class SkillService : BaseService<Skill, SkillDTO>
    {
        public SkillService(IUnitOfWork uow) : base(uow, uow.SkillRepo)
        {

        }
    }
}
