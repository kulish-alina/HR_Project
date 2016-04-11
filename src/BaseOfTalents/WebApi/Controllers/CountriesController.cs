using Domain.Entities.Setup;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.DTO.DTOModels;

namespace WebApi.Controllers
{
    public class CountriesController : BoTController<Country, Country>
    {
        public CountriesController(ICountryRepository countryRepository)
        {
            _repo = countryRepository;
        }
    }
}