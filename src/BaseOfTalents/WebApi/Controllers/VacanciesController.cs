using Service.Services;
using Domain.DTO.DTOModels;
using Domain.Entities;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class VacanciesController : BoTController<Vacancy, VacancyDTO>
    {
        public VacanciesController(IControllerService<Vacancy, VacancyDTO> service)
            : base(service)
        {
        }

        public VacanciesController()
        {
        }

        public override IHttpActionResult Add([FromBody]VacancyDTO vacancy)
        {
            if (!ModelState.IsValid)
            {
                StringBuilder errorString = new StringBuilder();
                foreach (var error in ModelState.Keys.SelectMany(k => ModelState[k].Errors))
                {
                    errorString.Append(error.ErrorMessage + '\n');
                }
                return BadRequest(errorString.ToString());
            }
            if (vacancy.Id != 0)
            {
                return BadRequest();
            }
            var addedVacancy = entityService.Add(vacancy);
            return Json(addedVacancy, BOT_SERIALIZER_SETTINGS);
        }


        public override IHttpActionResult Put(int id, [FromBody] VacancyDTO changedEntity)
        {
            if (!ModelState.IsValid)
            {
                StringBuilder errorString = new StringBuilder();
                foreach (var error in ModelState.Keys.SelectMany(k => ModelState[k].Errors))
                {
                    errorString.Append(error.ErrorMessage + '\n');
                }
                return BadRequest(errorString.ToString());
            }
            if (changedEntity.Id != id)
            {
                return BadRequest();
            }
            var changedVacancy = entityService.Put(changedEntity);
            return Json(changedVacancy, BOT_SERIALIZER_SETTINGS);
        }

        [HttpPost]
        [Route("api/vacancies/search")]
        public IHttpActionResult Search([FromBody]VacancySearchParameters searchParams)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var searchResult = entityService.Search(searchParams);
            if(searchResult==null) {
                return BadRequest("Check your Search Params");
            }
            return Json(searchResult, BOT_SERIALIZER_SETTINGS);
        }

        [HttpGet]
        // GET: api/Entities/
        public override IHttpActionResult Get()
        {
            return BadRequest("Get all is prohibited for vacancies. Use /search instead");
        }
    }
}
