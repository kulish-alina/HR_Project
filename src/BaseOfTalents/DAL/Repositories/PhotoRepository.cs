using BaseOfTalents.DAL.Infrastructure;
using BaseOfTalents.DAL.Repositories;
using BaseOfTalents.Domain.Entities;
using System.Data.Entity;

namespace DAL.Repositories
{
    public class PhotoRepository : BaseRepository<Photo>, IPhotoRepository
    {
        public PhotoRepository(DbContext context) : base(context)
        {

        }
    }
}
