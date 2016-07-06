using DAL.Infrastructure;
using Domain.Entities.Enum.Setup;
using System.Data.Entity;

namespace DAL.Repositories
{
    public class LanguageRepository : BaseRepository<Language>, ILanguageRepository
    {
        public LanguageRepository(DbContext context) : base(context)
        {
        }
    }
}