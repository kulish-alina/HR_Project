using System.Linq;
using Domain.Entities;
using Domain.Repositories;
using WebApi.DTO.DTOModels;

namespace WebApi.Controllers
{
    public class VacanciesController : BoTController<Vacancy, VacancyDTO>
    {
        public VacanciesController(IVacancyRepository vacancyRepository)
        {
            _repo = vacancyRepository;
        }
    }
}

