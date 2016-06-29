using System.Threading.Tasks;
using System.Web.Http;
using BaseOfTalents.WebUI.Extensions;
using BaseOfTalents.WebUI.Models;
using Domain.DTO.DTOModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WebUI.Filters;
using WebUI.Infrastructure.Auth;
using WebUI.Models;

namespace WebApi.Controllers
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
        /// Registers user with provided data
        /// </summary>
        /// <param name="user">User data</param>
        /// <returns>Updated user information</returns>
        [HttpPost, Auth]
        [Route("register")]
        public IHttpActionResult Register([FromBody] UserDTO user)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Errors();

                return Json(error, botSerializationSettings);
            }

            var result = _userAuthService.Register(user);
            return Json(result, botSerializationSettings);
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
            var login = model.Login;
            var password = model.Password;
            var data = await _userAuthService
                .LogInAsync(login, password);

            var result = new
            {
                Token = data.Item2,
                FirstName = data.Item1.FirstName,
                MiddleName = data.Item1.MiddleName,
                LastName = data.Item1.LastName,
                RoleId = data.Item1.RoleId,
                Photo = data.Item1.Photo,
                BirthDate = data.Item1.BirthDate,
                CreatedOn = data.Item1.CreatedOn,
                Login = data.Item1.Login
            };

            return Json(result, botSerializationSettings);
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
            bool logedOut = _userAuthService
                .LogOut(ActionContext.Request.Headers.Authorization.Parameter);
            return Json(logedOut, botSerializationSettings);
        }

        [HttpPost, AllowAnonymous]
        public IHttpActionResult Get([FromBody]IdentityModel identity)
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
                Login = user.Login
            };

            return Json(result, botSerializationSettings);
        }
    }
}
