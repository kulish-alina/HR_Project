using Domain.Entities.Setup;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.DTO.DTOModels;

namespace WebApi.Controllers
{
    public class CitiesController : BoTController<City, CityDTO>
    {
        public CitiesController(ICityRepository cityRepository)
        {
            _repo = cityRepository;

        }
    }
}