using BaseOfTalents.Domain.Entities;
using Domain.DTO.DTOModels;

namespace DAL.Extensions
{
    public static class PhotoExtensions
    {
        public static void Update(this Photo destination, PhotoDTO source)
        {
            destination.Description = source.Description;
            destination.ImagePath = source.ImagePath;
            destination.State = source.State;
        }
    }
}
