using BaseOfTalents.DAL.Infrastructure;
using BaseOfTalents.Domain.Entities.Enum.Setup;
using System.Data.Entity;

namespace BaseOfTalents.DAL.Repositories
{
    public class TagRepository : BaseRepository<Tag>, ITagRepository
    {
        public TagRepository(DbContext context) : base(context)
        {
        }
    }
}