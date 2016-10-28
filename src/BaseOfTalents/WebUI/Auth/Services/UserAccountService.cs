using System;
using System.Data.Entity.Core;
using System.Threading.Tasks;
using DAL.DTO;
using DAL.Services;
using Domain.Entities;
using Mailer;
using WebUI.Auth.Infrastructure;
using WebUI.Extensions;
using WebUI.Globals;
using WebUI.Globals.Validators;
using WebUI.Services;

namespace WebUI.Auth.Services
{
    using MailsService = BaseService<MailContent, MailDTO>;
    /// <summary>
    /// Service for managing user and its session
    /// </summary>
    public class UserAccountService : IAccountService
    {
        UserService _userService;
        MailsService _mailService;
        TemplateService _templateService;

        public UserAccountService(UserService userService, TemplateService templateService,
            MailsService mailService)
        {
            _userService = userService;
            _mailService = mailService;
            _templateService = templateService;
        }

        public UserDTO Register(UserDTO newUser, int mailId)
        {
            var mailContent = _mailService.Get(mailId);
            var template = _templateService.GetTemplate();

            string pwd = PasswordGenerator.GeneratePassword();
            newUser.Password = (Password)pwd;
            var addedUser = _userService.Add(newUser);

            var textAfterReplacing = MailBodyContentReplacer.Replace(mailContent.Body, addedUser.Login, pwd);
            var mail = MailTemplateGenerator.Generate(template, mailContent.Invitation, textAfterReplacing, mailContent.Farewell, mailContent.Subject,
                SettingsContext.Instance.GetImageUrl(), SettingsContext.Instance.GetOuterUrl());

            MailAgent.Send(addedUser.Email, mail.Subject, mail.Template);

            return addedUser;
        }

        /// <summary>
        /// Logs user out of application, clearing its session.
        /// </summary>
        /// <returns>True if action finished succesfully</returns>
        public bool LogOut(string token)
        {
            throw new NotImplementedException();
            try
            {

                //_authContainer.Delete(token);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Changing password for authenticated user by his token
        /// </summary>
        /// <param name="token">User's token</param>
        /// <param name="oldPassword">Old password for check user identity</param>
        /// <param name="newPassword">New password will be set to user's data if old password is match </param>
        public void ChangePassword(string token, string oldPassword, string newPassword)
        {
            int id = PayloadDecoder.TryGetId(token);
            var user = _userService.Get(id);
            var validator = new PasswordValidator();
            var result = validator.Validate(oldPassword, user.Password);
            if (!result.IsValid)
            {
                throw new ArgumentException(result.Message);
            }
            user.Password = (Password)newPassword;
            _userService.Update(user);
        }

        /// <summary>
        /// Perfoms accessing to user of specified login and password
        /// </summary>
        /// <param name="login">Application user login</param>
        /// <param name="password">User password (hashed)</param>
        /// <returns>Corresponting user dto object</returns>
        /// <exception cref="ArgumentException">Is thrown, when there is no user with such a login and password</exception>
        public UserDTO Authentificate(string login, string password)
        {
            var user = _userService.Get(login);
            if (user == null && user.Password.Equals(password))
            {
                throw new ArgumentException("Wrong login or password");
                //TODO: Extract message to external source
                //TODO: new exception type
            }
            return user;
        }

        public bool CkeckAuthority(string login, string password)
        {
            try
            {
                Authentificate(login, password);
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        /// <summary>
        /// Perfoms accessing to user of specified login and password async
        /// </summary>
        /// <param name="login">Application user login</param>
        /// <param name="password">User password (hashed)</param>
        /// <returns>Corresponting user dto object</returns>
        /// <exception cref="ArgumentException">Is thrown, when there is no user with such a login and password</exception>
        public async Task<UserDTO> AuthentificateAsync(string login, string password)
        {
            var user = await _userService.GetAsync(login);
            if (user == null && user.Password.Equals(password))
            {
                throw new ArgumentException("Wrong login or password");
                //TODO: Extract message to external source
                //TODO: new exception type
            }
            return user;
        }

        public async Task<bool> CkeckAuthorityAsync(string login, string password)
        {
            try
            {
                await AuthentificateAsync(login, password);
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
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

            string pwd = PasswordGenerator.GeneratePassword();
            _user.Password = (Password)pwd;
            _userService.Update(_user);

            var template = _templateService.GetTemplate();
            var mail = MailTemplateGenerator.Generate(template,
            "Hello! Your password was changed.",
            $"Your login is <b>{_user.Login}</b><br> Your new password is <b>{pwd}</b>",
            "Wish you a nice day!", "Password recovery",
            SettingsContext.Instance.GetImageUrl(), SettingsContext.Instance.GetOuterUrl());

            MailAgent.Send(_user.Email, mail.Subject, mail.Template);
        }
    }
}