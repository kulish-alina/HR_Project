using System;
using System.Threading.Tasks;
using DAL.DTO;
using DAL.Services;
using WebUI.Infrastructure.Auth;
using WebUI.Results;
using WebUI.Globals.Validators;

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

        public UserDTO GetUser(string token)
        {
            return _authContainer.Get(token).Item1;
        }

        /// <summary>
        /// Changing password for authenticated user by his token
        /// </summary>
        /// <param name="token">User's token</param>
        /// <param name="oldPassword">Old password for check user identity</param>
        /// <param name="newPassword">New password will be set to user's data if old password is match </param>
        public void ChangePassword(string token, string oldPassword, string newPassword)
        {
            var user = _userService.Get(this.GetUser(token).Id);
            var validator = new PasswordValidator();
            var result = validator.Validate(oldPassword, user.Password);
            if (!result.IsValid)
            {
                throw new ArgumentException(result.Message);
            }
            user.Password = newPassword;
            _userService.Update(user);
        }
    }
}