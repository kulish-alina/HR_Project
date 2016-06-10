using Domain.DTO.DTOModels;

namespace Domain.DTO.DTOModels.SetupDTO
{
    public class EventTypeDTO : BaseEntityDTO
    {
        public string Title { get; set; }
        public string ImagePath { get; set; }
    }
}
