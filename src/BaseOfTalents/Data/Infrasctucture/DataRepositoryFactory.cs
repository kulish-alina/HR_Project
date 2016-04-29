using Domain.Entities;
using System.Net.Http;
using Domain.Repositories;
using Data.EFData.Extentions;

namespace Data.Infrastructure
{
    public class DataRepositoryFactory : IDataRepositoryFactory
    {
        public IRepository<T> GetDataRepository<T>(HttpRequestMessage request) where T : BaseEntity, new()
        {
            return request.GetDataRepository<T>();
        }
    }
}
