using System;
using System.Web.Http;
using DAL.DTO;
using DAL.Exceptions;
using DAL.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WebUI.Auth;

namespace WebUI.Controllers
{
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        private UserService _service;

        private static JsonSerializerSettings botSerializationSettings = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
        public UserController(UserService service)
        {
            _service = service;
        }



        // GET api/<controller>
        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Json(_service.Get(), botSerializationSettings);
        }

        /// <summary>
        /// Api for getting corect user with session
        /// </summary>
        /// <param name="identity">the parameter for identifiing user</param>
        /// <returns>Full user info</returns>
        [HttpGet]
        [Route("bytoken")]
        public IHttpActionResult GetUser()
        {
            try
            {
                int userId = PayloadDecoder.TryGetId(ActionContext.Request.Headers.Authorization.Parameter);
                var user = _service.Get(userId);
                var result = new
                {
                    FirstName = user.FirstName,
                    MiddleName = user.MiddleName,
                    LastName = user.LastName,
                    RoleId = user.RoleId,
                    Photo = user.Photo,
                    BirthDate = user.BirthDate,
                    CreatedOn = user.CreatedOn,
                    Login = user.Login,
                    Email = user.Email,
                    Skype = user.Skype,
                    PhoneNumbers = user.PhoneNumbers,
                    IsMale = user.isMale,
                    CityId = user.CityId,
                    Id = user.Id
                };

                return Json(result, botSerializationSettings);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET api/<controller>/5
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetUser(int id)
        {
            var foundedEntity = _service.Get(id);
            if (foundedEntity == null)
            {
                return NotFound();
            }

            return Json(foundedEntity, botSerializationSettings);
        }
        // PUT api/<controller>/5
        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult Put(int id, [FromBody]UserDTO changedUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updatedUser = _service.Update(changedUser);
            return Json(updatedUser, botSerializationSettings);
        }

        // DELETE api/<controller>/5
        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                _service.Delete(id);
                return Ok();
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}
