using DAL.DTO;
using DAL.Exceptions;
using DAL.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Web.Http;
using WebUI.Filters;
using WebUI.Models;

namespace WebUI.Controllers
{
    [RoutePrefix("api/vacancy")]
    public class VacancyController : ApiController
    {
        private VacancyService service;
        private static JsonSerializerSettings BOT_SERIALIZER_SETTINGS = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
        public VacancyController(VacancyService service)
        {
            this.service = service;
        }

        public VacancyController()
        {

        }

        // GET api/<controller>
        [HttpPost]
        [Route("search")]
        [Auth, PermissionAuthorization(Permissions = Domain.Entities.Enum.AccessRight.ViewListOfVacancies)]
        public IHttpActionResult Get([FromBody]VacancySearchParameters vacancyParams)
        {
            if (ModelState.IsValid)
            {
                var tupleResult = service.Get(
                    vacancyParams.UserId,
                    vacancyParams.IndustryId,
                    vacancyParams.Title,
                    vacancyParams.VacancyState,
                    vacancyParams.TypeOfEmployment,
                    vacancyParams.LevelIds,
                    vacancyParams.CityIds,
                    vacancyParams.Current,
                    vacancyParams.Size
                    );

                var vacanciesViewModel = tupleResult.Item1;
                var total = tupleResult.Item2;

                var ret = new { Vacancies = vacanciesViewModel, Current = vacancyParams.Current, Size = vacancyParams.Size, Total = total };
                return Json(ret, BOT_SERIALIZER_SETTINGS);
            }
            return BadRequest(ModelState);
        }

        // GET api/<controller>/5
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            var foundedEntity = service.Get(id);
            if (foundedEntity == null)
            {
                ModelState.AddModelError("Vacancy", "The entity with id: " + id + " not found.");
                return BadRequest(ModelState);
            }
            return Json(foundedEntity, BOT_SERIALIZER_SETTINGS);
        }

        // POST api/<controller>
        [HttpPost]
        [Route("")]
        [Auth, PermissionAuthorization(Permissions = Domain.Entities.Enum.AccessRight.AddVacancy)]
        public IHttpActionResult Post([FromBody]VacancyDTO vacancy)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updatedVacancy = service.Add(vacancy);
            return Json(updatedVacancy, BOT_SERIALIZER_SETTINGS);
        }

        // PUT api/<controller>/5
        [HttpPut]
        [Route("{id}")]
        [Auth, PermissionAuthorization(Permissions = Domain.Entities.Enum.AccessRight.EditVacancy)]
        public IHttpActionResult Put(int id, [FromBody]VacancyDTO vacancy)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updatedVacancy = service.Update(vacancy);
            return Json(updatedVacancy, BOT_SERIALIZER_SETTINGS);
        }

        // DELETE api/<controller>/5
        [HttpDelete]
        [Route("{id:int}")]
        [Auth, PermissionAuthorization(Permissions = Domain.Entities.Enum.AccessRight.RemoveVacancy)]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                service.Delete(id);
                return Ok();
            }
            catch (EntityNotFoundException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Get(new VacancySearchParameters());
        }
    }
}