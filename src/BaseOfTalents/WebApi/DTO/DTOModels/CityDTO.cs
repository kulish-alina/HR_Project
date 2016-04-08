using Domain.Entities.Enum;
using Domain.Entities.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.DTO.DTOModels
{
    public class CityDTO
    {
        public int Id { get; set; }
        public DateTime EditTime { get; set; }
        public EntityState State { get; set; }
        public string Name { get; set; }
        public virtual Country Country { get; set; }
    }
}