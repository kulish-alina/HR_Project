using Domain.Entities.Enum;
using System.Collections.Generic;

namespace DAL.DTO
{
    public class PermissionDTO : BaseEntityDTO
    {
        public string Description { get; set; }
        public AccessRight AccessRights { get; set; }
        public string Group { get; set; }

        public IEnumerable<int> RoleIds { get; set; }
    }
}
