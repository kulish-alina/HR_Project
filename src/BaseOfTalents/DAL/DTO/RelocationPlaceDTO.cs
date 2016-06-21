using Domain.DTO.DTOModels;
using System.Collections.Generic;

namespace DAL.DTO
{
    public class RelocationPlaceDTO : BaseEntityDTO
    {
        public int CountryId { get; set; }
        public IEnumerable<int> CityIds { get; set; }
    }
}
