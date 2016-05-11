using Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            State = EntityState.Active;
        }

        public int Id { get; set; }
        public DateTime? EditTime { get; set; }
        public EntityState State { get; set; }
    }
}
