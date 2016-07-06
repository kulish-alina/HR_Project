using DAL.DTO;
using System;

namespace WebUI.Infrastructure.Auth
{
    /// <summary>
    /// The contract of where and how users and their sessions are stored.
    /// </summary>
    /// <typeparam name="T">T is type of credentials (for example token and password)</typeparam>
    public interface IAuthContainer<T>
    {
        /// <summary>
        /// Returns the user registered with specified credentials
        /// </summary>
        /// <param name="credentials">The value, information is stored with</param>
        /// <returns></returns>
        Tuple<UserDTO, RoleDTO> Get(T credentials);

        /// <summary>
        /// Creates new session info for user
        /// </summary>
        /// <param name="user">User to create session</param>
        /// <param name="role">Info of its rights/claims</param>
        /// <param name="credentials">The key </param>
        void Put(UserDTO user, RoleDTO role, T credentials);

        /// <summary>
        /// Deletes user's session
        /// </summary>
        /// <param name="credentials">User uniq key (like token)</param>
        void Delete(T credentials);

        /// <summary>
        /// The type of credentials scheme
        /// </summary>
        string Scheme { get; }
    }
}
