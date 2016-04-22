using Data.EFData.Design;
using Domain.Entities.Setup;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Controllers
{
    public class SkillsController : BoTController<Skill, Skill>
    {
        public SkillsController(IRepositoryFacade facade) : base(facade)
        {
            _currentRepo = _repoFacade.SkillRepository;
        }
    }
}