using System;
using System.Threading.Tasks;
using DAL.DTO;
using WebUI.Results;

namespace WebUI.Infrastructure.Auth
{
    /// <summary>
    /// Service of controlling user actions, such as login (signin), registration and logout
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Performs first authentification of user
        /// </summary>
        /// <param name="login">The login user provided in form</param>
        /// <param name="password">User's password</param>
        /// <returns>The user and some kind of credentials as the second part</returns>
        Task<Tuple<UserDTO, string>> LogInAsync(string login, string password);

        /// <summary>
        /// Performs a logout - the action opposite to login
        /// </summary>
        /// <returns>True if action finished successfully. Else false.</returns>
        bool LogOut(string token);

        UserDTO GetUser(string token);
        void ChangePassword(string token, string oldPassword, string newPassword);
        void RecoverAccount(string loginOrEmail);
    }
}
