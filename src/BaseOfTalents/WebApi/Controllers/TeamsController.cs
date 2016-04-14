using Domain.Entities.Setup;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Controllers
{
    public class TeamsController : BoTController<Team, Team>
    {
        public TeamsController(ITeamRepository teamRepository)
        {
            _repo = teamRepository;
        }
    }
}