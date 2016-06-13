using BaseOfTalents.WebUI.Extensions;
using BaseOfTalents.WebUI.Models;
using DAL.Exceptions;
using DAL.Services;
using Domain.DTO.DTOModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Web.Http;

namespace BaseOfTalents.WebUI.Controllers
{
    [RoutePrefix("api/event")]
    public class EventController : ApiController
    {
        private EventService service;
        private static JsonSerializerSettings BOT_SERIALIZER_SETTINGS = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
        public EventController(EventService service)
        {
            this.service = service;
        }
        public EventController()
        {

        }

        // GET api/<controller>
        [HttpPost]
        [Route("search")]
        public IHttpActionResult Get([FromBody]string paramss)
        {
            /*if (ModelState.IsValid)
            {
                var tupleResult = service.Get(
                    vacancyParams.UserId,
                    vacancyParams.IndustryId,
                    vacancyParams.Title,
                    vacancyParams.VacancyState,
                    vacancyParams.TypeOfEmployment,
                    vacancyParams.LevelIds,
                    vacancyParams.LocationIds,
                    vacancyParams.Current,
                    vacancyParams.Size
                    );

                var vacanciesViewModel = tupleResult.Item1;
                var total = tupleResult.Item2;

                var ret = new { Vacancies = vacanciesViewModel, Current = vacancyParams.Current, Size = vacancyParams.Size, Total = total };
                return Json(ret, BOT_SERIALIZER_SETTINGS);
            }*/
            return BadRequest("Not implemented");
        }

        // GET api/<controller>/5
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            var foundedEntity = service.Get(id);
            if (foundedEntity != null)
            {
                return Json(foundedEntity, BOT_SERIALIZER_SETTINGS);
            }
            return BadRequest();
        }

        // POST api/<controller>
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody]EventDTO newEvent)
        {
            if (!ModelState.IsValid)
            {
                return Json(ModelState.Errors(), BOT_SERIALIZER_SETTINGS);
            }
            var addedEvent = service.Add(newEvent);
            return Json(addedEvent, BOT_SERIALIZER_SETTINGS);
        }

        // PUT api/<controller>/5
        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult Put(int id, [FromBody]EventDTO changedEvent)
        {
            if (!ModelState.IsValid)
            {
                return Json(ModelState.Errors(), BOT_SERIALIZER_SETTINGS);
            }
            var updatedEvent = service.Update(changedEvent);
            return Json(updatedEvent, BOT_SERIALIZER_SETTINGS);
        }

        // DELETE api/<controller>/5
        [HttpDelete]
        [Route("{id:int}")]
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
    }
}
