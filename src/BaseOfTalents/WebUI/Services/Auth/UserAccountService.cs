using System;
using System.Threading.Tasks;
using DAL.DTO;
using DAL.Services;
using WebUI.Infrastructure.Auth;
using WebUI.Results;
using WebUI.Globals.Validators;
using System.Data.Entity.Core;
using WebUI.Globals;
using Mailer;
using WebUI.Extensions;

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
        TemplateService _templateService;

        public UserAccountService(UserService userService, RoleService roleService, IAuthContainer<string> authContainer, TemplateService templateService)
        {
            _userService = userService;
            _roleService = roleService;
            _authContainer = authContainer;
            _templateService = templateService;
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

        /// <summary>
        /// Recovering account by email or login
        /// </summary>
        /// <param name="loginOrEmail">string contains user's login or email</param>
        public void RecoverAccount(string loginOrEmail)
        {
            if (String.IsNullOrEmpty(loginOrEmail))
            {
                throw new ArgumentException("Login and email can not be empty!");
            }

            var _validator = new EmailStringValidator();
            UserDTO _user = _userService.Get((usr) => _validator.IsEmail(loginOrEmail) ?
                                                              usr.Email == loginOrEmail :
                                                              usr.Login == loginOrEmail);

            if (_user == null)
            {
                throw new ObjectNotFoundException("User with such login or email not found!");
            }

            PasswordGenerator.GeneratePassword(_user);
            _userService.Update(_user);

            var template = _templateService.GetTemplate();
            var mail = MailTemplateGenerator.Generate(template,
            "Hello! Your password was changed.",
            $"Your login is <b>{_user.Login}</b><br> Your new password is <b>{_user.Password}</b>",
            "Wish you a nice day!", "Password recovery",
            SettingsContext.Instance.GetImageUrl(), SettingsContext.Instance.GetOuterUrl());

        MailAgent.Send(_user.Email, mail.Subject, mail.Template);
        }
    }
}