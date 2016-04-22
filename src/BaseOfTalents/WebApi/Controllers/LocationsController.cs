using Data.EFData.Design;
using Domain.Entities.Setup;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Controllers
{
    public class LocationsController : BoTController<Location, Location>
    {
        public LocationsController(IRepositoryFacade facade) : base(facade)
        {
            _currentRepo = _repoFacade.CityRepository;
        }
    }
}