using System.Linq;
using Domain.Entities;
using Domain.Repositories;
using Domain.DTO.DTOModels;
using Data.EFData.Design;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class VacanciesController : BoTController<Vacancy, VacancyDTO>
    {
        public VacanciesController(IRepositoryFacade facade) : base(facade)
        {
            _currentRepo = _repoFacade.VacancyRepository;
        }
      
    }
}

