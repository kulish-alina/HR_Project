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
        public int Id { get; set; }
        public EntityState State { get; set; }
    }
}
