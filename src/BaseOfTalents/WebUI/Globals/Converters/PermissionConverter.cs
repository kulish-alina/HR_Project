using System.Collections.Generic;
using System.Linq;
using BaseOfTalents.Domain.Entities.Enum;
using BaseOfTalents.Domain.Entities.Enum.Setup;

namespace BaseOfTalents.WebUI.Globals.Converters
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