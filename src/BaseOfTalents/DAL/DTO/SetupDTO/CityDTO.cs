using Domain.DTO.DTOModels;


namespace Domain.DTO.DTOModels.SetupDTO
{
    public class CityDTO : BaseEntityDTO
    {
        public string Title { get; set; }

        public int CountryId { get; set; }
    }
}
