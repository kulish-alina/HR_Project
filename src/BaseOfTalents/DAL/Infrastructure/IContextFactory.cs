using System.Data.Entity;

namespace DAL.Infrastructure
{
    public interface IContextFactory
    {
        DbContext Create();
        DbContext Create(string dbName, string dataSource);
        DbContext Create(string connectionString);
    }
}
