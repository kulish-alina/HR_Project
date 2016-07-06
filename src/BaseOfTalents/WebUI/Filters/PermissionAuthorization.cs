using Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace WebUI.Filters
{
    public class PermissionAuthorizationAttribute : AuthorizeAttribute
    {
        public AccessRight Permissions { get; set; }

        /// <summary>
        /// Main action to check if user has authorities to acesss
        /// </summary>
        /// <param name="actionContext">Context of user trying to access resouce</param>
        /// <returns></returns>
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var principal = actionContext.Request
                .GetRequestContext()
                .Principal as ClaimsPrincipal;

            return hasClaim(principal?.Claims);
        }

        /// <summary>
        /// Checks on users permission on access to action
        /// </summary>
        /// <param name="claims">User claims (rights)</param>
        /// <returns>True if user can access to specified action. Else false.</returns>
        private bool hasClaim(IEnumerable<Claim> claims)
        {
            return claims.Any(claim =>
                    isTypeCorrect(claim.Type) &&
                    hasFlag(claim.Value, Permissions));
        }

        /// <summary>
        /// The function that checks on corresponding claim type
        /// </summary>
        /// <param name="type">Claim's type</param>
        /// <returns>True or false according to is the type correct (corresponding) to the one needed</returns>
        private static bool isTypeCorrect(string type)
        {
            return type == "Permission";
        }

        /// <summary>
        /// Checking on if user has in its claims the specific one neeeded
        /// </summary>
        /// <param name="value">Value of Permissions claim</param>
        /// <param name="permission">The permission for specific action</param>
        /// <returns>True if there is such a permission for user</returns>
        private static bool hasFlag(string value, AccessRight permission)
        {
            return ((AccessRight)Enum.Parse(typeof(AccessRight), value))
                    .HasFlag(permission);
        }
    }
}