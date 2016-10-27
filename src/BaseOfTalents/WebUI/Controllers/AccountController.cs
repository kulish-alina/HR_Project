using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WebUI.Filters;
using WebUI.Infrastructure.Auth;
using WebUI.Models;

namespace WebUI.Controllers
{
    /// <summary>
    /// Controller of user actions, like registration, login/logout
    /// </summary>
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        private IAccountService _userAuthService;
        private static JsonSerializerSettings botSerializationSettings = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public AccountController(IAccountService userAuthService)
        {
            _userAuthService = userAuthService;
        }

        /// <summary>
        /// Logs user into application and creates
        /// </summary>
        /// <param name="model">User data</param>
        /// <returns>Credentials and user data</returns>
        [HttpPost, AllowAnonymous]
        [Route("signin")]
        public async Task<IHttpActionResult> Signin([FromBody]LoginModel model)
        {
            try
            {
                var login = model.Login;
                var password = model.Password;
                var data = await _userAuthService
                    .LogInAsync(login, password);

                var user = data.Item1;
                string token = data.Item2;

                var result = new
                {
                    Token = token,
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
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        /// <summary>
        /// Logs user out of the application.
        /// Deletes session.
        /// </summary>
        /// <returns>Success or unsuccess</returns>
        [HttpPost, Auth]
        [Route("logout")]
        public IHttpActionResult Logout()
        {
            try
            {
                bool logedOut = _userAuthService
                    .LogOut(ActionContext.Request.Headers.Authorization.Parameter);
                return Json(logedOut, botSerializationSettings);
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        /// <summary>
        /// Api for getting corect user with session
        /// </summary>
        /// <param name="identity">the parameter for identifiing user</param>
        /// <returns>Full user info</returns>
        [HttpPost, AllowAnonymous]
        [Route("")]
        public IHttpActionResult Get([FromBody]IdentityModel identity)
        {
            try
            {
                var user = _userAuthService.GetUser(identity.Token);
                var result = new
                {
                    Token = identity.Token,
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
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Api for changing user password
        /// </summary>
        [HttpPost, Auth]
        [Route("password")]
        public IHttpActionResult ChangePassword([FromBody]ChangePasswordModel model)
        {
            if(ModelState.IsValid)
            {
                var result = _userAuthService.ChangePassword(ActionContext.Request.Headers.Authorization.Parameter, model.OldPassword, model.NewPassword);
                return result.Result ? Ok() as IHttpActionResult : BadRequest(result.Message);
            }
            return BadRequest(ModelState);
        }
    }
}