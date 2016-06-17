using Domain.DTO.DTOModels;
using Domain.DTO.DTOModels.SetupDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class RelocationPlaceDTO : BaseEntityDTO
    {
        public int CountryId { get; set; }
        public IEnumerable<int> LocationIds { get; set; }
    }
}
