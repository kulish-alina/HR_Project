using DAL.Infrastructure;
using Domain.Entities;
using System.Data.Entity;

namespace DAL.Repositories
{
    public class FileRepository : BaseRepository<File>, IFileRepository
    {
        public FileRepository(DbContext context) : base(context)
        {
        }
    }
}