using Data.EFData.Repositories;
using Domain.Entities;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Data.Infrastructure
{
    public interface IDataRepositoryFactory
    {
        IRepository<T> GetDataRepository<T>(HttpRequestMessage request) where T : BaseEntity, new();
    }
}
