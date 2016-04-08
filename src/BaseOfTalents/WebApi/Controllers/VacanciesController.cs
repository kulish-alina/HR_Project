using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Domain.Entities;
using Domain.Entities.Enum;
using Domain.Entities.Setup;
using Domain.Repositories;
using WebApi.DTO;
using WebApi.DTO.DTOModels;
using WebApi.DTO.DTOService.Abstract;

namespace WebApi.Controllers
{
    public class VacanciesController : BoTController<Vacancy, VacancyDTO>
    {
        public VacanciesController(IVacancyRepository vacancyRepository, IVacancyDTOService vacancyDTOService)
        {
            _repo = vacancyRepository;
            _dtoService = vacancyDTOService;
        }
    }
}

