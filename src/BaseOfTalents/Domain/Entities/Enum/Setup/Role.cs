using System.Collections.Generic;

namespace BaseOfTalents.Domain.Entities.Enum.Setup
{
    public class Role : BaseEntity
    {
        public Role()
        {
            Permissions = new List<Permission>();
        }

        public string Title { get; set; }

        public virtual ICollection<Permission> Permissions { get; set; }
    }
}