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
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        private UserService service;
        private static JsonSerializerSettings BOT_SERIALIZER_SETTINGS = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
        public UserController(UserService service)
        {
            this.service = service;
        }

        public UserController()
        {

        }

        // GET api/<controller>
        [HttpPost]
        [Route("search")]
        public IHttpActionResult Get([FromBody]string paramss)
        {
            return Json(service.Get(new object()), BOT_SERIALIZER_SETTINGS);
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            return this.Get("");
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
        public IHttpActionResult Post([FromBody]UserDTO newUser)
        {
            if (!ModelState.IsValid)
            {
                return Json(ModelState.Errors(), BOT_SERIALIZER_SETTINGS);
            }
            var addedUser = service.Add(newUser);
            return Json(addedUser, BOT_SERIALIZER_SETTINGS);
        }

        // PUT api/<controller>/5
        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult Put(int id, [FromBody]UserDTO changedUser)
        {
            if (!ModelState.IsValid)
            {
                return Json(ModelState.Errors(), BOT_SERIALIZER_SETTINGS);
            }
            var updatedUser = service.Update(changedUser);
            return Json(updatedUser, BOT_SERIALIZER_SETTINGS);
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
