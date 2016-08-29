using DAL.DTO;
using DAL.Exceptions;
using DAL.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Web.Http;
using WebUI.Extensions;

namespace WebUI.Controllers
{
    [RoutePrefix("api/note")]
    public class NoteController : ApiController
    {
        private NoteService service;
        private static JsonSerializerSettings BOT_SERIALIZER_SETTINGS = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
        public NoteController(NoteService service)
        {
            this.service = service;
        }

        public NoteController()
        {
        }

        // GET api/<controller>
        [HttpGet]
        [Route("user/{id}")]
        public IHttpActionResult GetByUser(int id)
        {
            var foundedNotes = service.GetByUserId(id);
            if (foundedNotes == null)
            {
                ModelState.AddModelError("Note", "Note with id " + id + " not found");
                return BadRequest(ModelState);
            }
            return Json(foundedNotes, BOT_SERIALIZER_SETTINGS);
        }


        // POST api/<controller>
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody]NoteDTO noteToAdd)
        {
            if (!ModelState.IsValid)
            {
                return Json(ModelState.Errors(), BOT_SERIALIZER_SETTINGS);
            }
            var addedNote = service.Add(noteToAdd);
            return Json(addedNote, BOT_SERIALIZER_SETTINGS);
        }

        // PUT api/<controller>/5
        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult Put(int id, [FromBody]NoteDTO changedNote)
        {
            if (!ModelState.IsValid)
            {
                return Json(ModelState.Errors(), BOT_SERIALIZER_SETTINGS);
            }
            var updatedNote = service.Update(changedNote);
            return Json(updatedNote, BOT_SERIALIZER_SETTINGS);
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
