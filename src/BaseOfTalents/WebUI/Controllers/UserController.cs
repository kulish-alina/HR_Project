using System.Web.Http;
using DAL.DTO;
using DAL.Exceptions;
using DAL.Services;
using Domain.Entities;
using Mailer;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WebUI.Extensions;
using WebUI.Globals;
using WebUI.Models;
using WebUI.Services;
using WebUI.Results;

namespace WebUI.Controllers
{
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        private UserService _service;
        private BaseService<MailContent, MailDTO> _mailService;
        private TemplateService _templateService;

        private static JsonSerializerSettings BOT_SERIALIZER_SETTINGS = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
        public UserController(UserService service, BaseService<MailContent, MailDTO> mailService,
            TemplateService templateService)
        {
            _service = service;
            _mailService = mailService;
            _templateService = templateService;
        }

        // GET api/<controller>
        [HttpPost]
        [Route("search")]
        public IHttpActionResult Get([FromBody]string paramss)
        {
            return Json(_service.Get(new object()), BOT_SERIALIZER_SETTINGS);
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
            var foundedEntity = _service.Get(id);
            if (foundedEntity == null)
            {
                ModelState.AddModelError("User", "User with id " + id + " not found.");
                return BadRequest(ModelState);
            }
            return Json(foundedEntity, BOT_SERIALIZER_SETTINGS);
        }

        // POST api/<controller>
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody]RegistrationModel newUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mailContent = _mailService.Get(newUser.MailId);
            var template = _templateService.GetTemplate();

            newUser.GeneratePassword();
            UserDTO addedUser;
            try
            {
                addedUser = _service.Add(newUser);
            }
            catch (System.ArgumentException e)
            {
                return new ConflictResult(e.Message);
            }

            var textAfterReplacing = MailBodyContentReplacer.Replace(mailContent.Body, addedUser.Login, addedUser.Password);
            var mail = MailTemplateGenerator.Generate(template, mailContent.Invitation, textAfterReplacing, mailContent.Farewell, mailContent.Subject,
                SettingsContext.Instance.GetImageUrl(), SettingsContext.Instance.GetOuterUrl());

            MailAgent.Send(addedUser.Email, mail.Subject, mail.Template);
            return Json(addedUser, BOT_SERIALIZER_SETTINGS);
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
            return Json(updatedUser, BOT_SERIALIZER_SETTINGS);
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
            catch (EntityNotFoundException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
