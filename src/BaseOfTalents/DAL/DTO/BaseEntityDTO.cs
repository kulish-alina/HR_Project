using System;
using Domain.Entities.Enum;

namespace DAL.DTO
{
    public abstract class BaseEntityDTO
    {
        protected BaseEntityDTO()
        {
            State = EntityState.Active;
        }
        public int Id { get; set; }
        public DateTime? CreatedOn { get; set; }
        public EntityState State { get; set; }

        public bool ShouldBeRemoved()
        {
            return State == EntityState.Inactive;
        }

        public bool IsNew()
        {
            return Id == 0;
        }
    }
}
