using Domain.DTO.DTOModels;


namespace Domain.DTO.DTOModels.SetupDTO
{
    public class LocationDTO : BaseEntityDTO
    {
        public string Title { get; set; }

        public int CountryId { get; set; }
    }
}
