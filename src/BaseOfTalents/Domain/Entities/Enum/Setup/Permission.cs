using System.Collections.Generic;

namespace BaseOfTalents.Domain.Entities.Enum.Setup
{
    public class Permission : BaseEntity
    {
        public Permission()
        {
            Roles = new List<Role>();
        }

        public string Description { get; set; }
        public AccessRights AccessRights { get; set; }
        public string Group { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
    }
}