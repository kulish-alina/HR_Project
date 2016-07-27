using DAL.DTO;
using DAL.Exceptions;
using DAL.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Web.Http;
using WebUI.Extensions;
using WebUI.Models;

namespace WebUI.Controllers
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
        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            ModelState.AddModelError("Search", "Go to /search and specify search parameters");
            return BadRequest(ModelState);
        }

        [HttpPost]
        [Route("search")]
        public IHttpActionResult Get([FromBody]EventSearchParameteres searchParams)
        {
            if (!searchParams.EndDate.HasValue && searchParams.StartDate.Day != 1)
            {
                ModelState.AddModelError("Get month events", "if you want to get all of the events for a month endDate must me NULL, startDate.day must be 1");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var foundedEvents = service.Get(searchParams.UserIds, searchParams.StartDate, searchParams.EndDate);
            return Json(foundedEvents, BOT_SERIALIZER_SETTINGS);
        }


        [HttpGet]
        [Route("candidate/{candidateId:int}")]
        public IHttpActionResult Get(int candidateid)
        {
            var foundedEvents = service.GetByCandidateId(candidateid);
            return Json(foundedEvents, BOT_SERIALIZER_SETTINGS);
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
