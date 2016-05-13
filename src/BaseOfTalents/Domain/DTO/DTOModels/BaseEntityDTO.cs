using Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.DTOModels
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
