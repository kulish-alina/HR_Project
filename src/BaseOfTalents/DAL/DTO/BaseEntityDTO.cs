using Domain.Entities.Enum;
using System;

namespace DAL.DTO
{
    public class BaseEntityDTO
    {
        public BaseEntityDTO()
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
