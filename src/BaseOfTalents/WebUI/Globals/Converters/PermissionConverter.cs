using Domain.Entities.Enum;
using Domain.Entities.Enum.Setup;
using System.Collections.Generic;
using System.Linq;

namespace WebUI.Globals.Converters
{
    public static class PermissionConverter
    {
        /// <summary>
        /// Specific function that converts the list of permission to just one access right
        /// </summary>
        /// <param name="permissions">The list of permissions that should be converted</param>
        /// <returns>The only value of AccessRight</returns>
        public static AccessRight Convert(IEnumerable<Permission> permissions)
        {
            return permissions.Aggregate(AccessRight.None, (res, claim) => res |= claim.AccessRights);
        }
    }
}