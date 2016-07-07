using DAL.DTO;
using Domain.Entities;

namespace DAL.Extensions
{
    public static class FileExtension
    {
        public static void Update(this File destination, FileDTO source)
        {
            destination.Description = source.Description;
        }
    }
}
