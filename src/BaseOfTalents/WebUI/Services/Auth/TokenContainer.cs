using DAL.DTO;
using System;
using System.Collections.Generic;
using WebUI.Infrastructure.Auth;

namespace WebUI.Services
{
    /// <summary>
    /// Token storage with user session info
    /// </summary>
    public class TokenContainer : IAuthContainer<string>
    {
        private static Dictionary<string, Tuple<UserDTO, RoleDTO>> cache
            = new Dictionary<string, Tuple<UserDTO, RoleDTO>>();

        /// <summary>
        /// The type of credentials stored in cache
        /// </summary>
        public string Scheme
        {
            get
            {
                return "Token";
            }
        }

        /// <summary>
        /// The function to get access to logged users
        /// </summary>
        /// <param name="credentials">The key to get access to user</param>
        /// <returns></returns>
        public Tuple<UserDTO, RoleDTO> Get(string credentials)
        {
            if (!cache.ContainsKey(credentials))
            {
                throw new Exception("there is no such a user logged into");
            }
            return cache[credentials];
        }

        /// <summary>
        /// Sets up info that user is logged into the system and now gets access to its actions.
        /// </summary>
        /// <param name="user">User information</param>
        /// <param name="role">User claims</param>
        /// <param name="credentials">The key value for user, specified in authentification scenario</param>
        public void Put(UserDTO user, RoleDTO role, string credentials)
        {
            cache.Add(credentials, Tuple.Create(user, role));
        }

        /// <summary>
        /// Deletes user session
        /// </summary>
        /// <param name="credentials">User specific authorization key</param>
        public void Delete(string credentials)
        {
            if (!cache.ContainsKey(credentials))
            {
                throw new Exception("there is no such a user logged into");
            }
            cache.Remove(credentials);
        }
    }
}