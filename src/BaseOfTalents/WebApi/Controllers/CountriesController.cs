using Data.EFData.Design;
using Domain.Entities.Setup;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Controllers
{
    public class CountriesController : BoTController<Country, Country>
    {
        public CountriesController(IRepositoryFacade facade) : base(facade)
        {
            _currentRepo = _repoFacade.CountryRepository;
        }
    }
}