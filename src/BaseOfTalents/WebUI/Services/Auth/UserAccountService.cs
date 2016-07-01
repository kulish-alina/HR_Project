using DAL.DTO;
using DAL.Services;
using System;
using System.Threading.Tasks;
using WebUI.Filters;
using WebUI.Infrastructure.Auth;

namespace WebUI.Services.Auth
{
    /// <summary>
    /// Service for managing user and its session
    /// </summary>
    public class UserAccountService : IAccountService
    {
        UserService _userService;
        RoleService _roleService;
        IAuthContainer<string> _authContainer;

        public UserAccountService(UserService userService, RoleService roleService, IAuthContainer<string> authContainer)
        {
            _userService = userService;
            _roleService = roleService;
            _authContainer = authContainer;
        }

        /// <summary>
        /// Logs user into the application and registers new session for they.
        /// </summary>
        /// <param name="login">User login</param>
        /// <param name="password">User password</param>
        /// <returns>User token and user data</returns>
        public async Task<Tuple<UserDTO, string>> LogInAsync(string login, string password)
        {
            var user = await _userService
                .AuthentificateAsync(login, password);

            var role = _roleService
                .Get(user.RoleId);

            string token = TokenProvider
                .CreateToken();

            _authContainer.Put(user, role, token);
            //Cashing user

            return Tuple.Create(user, token);
        }

        /// <summary>
        /// Logs user out of application, clearing its session.
        /// </summary>
        /// <returns>True if action finished succesfully</returns>
        [Auth]
        public bool LogOut(string token)
        {
            try
            {
                _authContainer.Delete(token);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Registers user with provided data
        /// </summary>
        /// <param name="model">User data</param>
        /// <returns>Updated user data</returns>
        public UserDTO Register(UserDTO model)
        {
            UserDTO modelResult = _userService.Add(model);
            return modelResult;
        }

        public UserDTO GetUser(string token)
        {
            return _authContainer.Get(token).Item1;
        }
    }
}