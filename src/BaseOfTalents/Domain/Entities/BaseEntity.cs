using BaseOfTalents.Domain.Entities.Enum;
using System;

namespace BaseOfTalents.Domain.Entities
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            State = EntityState.Active;
        }

        public int Id { get; set; }
        public DateTime? LastModified { get; set; }
        public DateTime? CreatedOn { get; set; }
        public EntityState State { get; set; }

        public bool? IsDeleted { get; set; }
    }
}