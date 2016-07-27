using System.Collections.Generic;

namespace Domain.Entities.Enum.Setup
{
    public class Permission : BaseEntity
    {
        public Permission()
        {
            Roles = new List<Role>();
        }

        public string Description { get; set; }
        public AccessRight AccessRights { get; set; }
        public string Group { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
    }
}