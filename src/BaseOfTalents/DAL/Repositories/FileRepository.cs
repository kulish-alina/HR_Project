using BaseOfTalents.DAL.Infrastructure;
using BaseOfTalents.Domain.Entities;
using System.Data.Entity;

namespace BaseOfTalents.DAL.Repositories
{
    public class FileRepository : BaseRepository<File>, IFileRepository
    {
        public FileRepository(DbContext context) : base(context)
        {
        }
    }
}