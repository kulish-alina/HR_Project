using Domain.Entities.Setup;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.DTO.DTOModels;

namespace WebApi.Controllers
{
    public class CitiesController : BoTController<City, City>
    {
        public CitiesController(ICityRepository cityRepository)
        {
            _repo = cityRepository;

        }
    }
}