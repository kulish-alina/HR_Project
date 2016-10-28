using System;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WebUI.Auth.Infrastructure;
using WebUI.Models;
using WebUI.Results;

namespace WebUI.Controllers
{
    /// <summary>
    /// Controller of user actions, like registration, login/logout
    /// </summary>
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        private IAccountService _userAccountService;

        private static JsonSerializerSettings botSerializationSettings = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public AccountController(IAccountService userAccountService)
        {
            _userAccountService = userAccountService;
        }

        // POST api/<controller>
        [HttpPost]
        [Route("invite"), Authorize]
        public IHttpActionResult Invite([FromBody]RegistrationModel newUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var addedUser = _userAccountService.Register(newUser, newUser.Id);
            return Json(addedUser, botSerializationSettings);
        }

        /// <summary>
        /// Logs user out of the application.
        /// Deletes session.
        /// </summary>
        /// <returns>Success or unsuccess</returns>
        [HttpPost]
        [Route("logout"), Authorize]
        public IHttpActionResult Logout()
        {
            try
            {
                bool logedOut = _userAccountService
                    .LogOut(ActionContext.Request.Headers.Authorization.Parameter);
                return logedOut ? Unauthorized() : BadRequest() as IHttpActionResult;
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        /// <summary>
        /// Api for changing user password
        /// </summary>
        [HttpPost]
        [Route("password"), Authorize]
        public IHttpActionResult ChangePassword([FromBody]ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                _userAccountService.ChangePassword(ActionContext.Request.Headers.Authorization.Parameter, model.OldPassword, model.NewPassword);
                return Ok();
            }
            catch (ArgumentException e)
            {
                return new ForbiddenResult(e.Message);
            }
        }

        /// <summary>
        /// Api for recovering accounts
        /// </summary>
        [HttpPost, AllowAnonymous]
        [Route("recover")]
        public IHttpActionResult RecoverAccount([FromBody]string loginOrEmail)
        {
            try
            {
                _userAccountService.RecoverAccount(loginOrEmail);
                return Ok();
            }
            catch (Exception e)
            {
                return new ForbiddenResult(e.Message);
            }
        }
    }
}