using Domain.Entities;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Dependencies;

namespace Data.EFData.Extentions
{
    public static class RequestMessageExtensions
    {
        private static TService GetService<TService>(this HttpRequestMessage request)
        {
            IDependencyScope dependencyScope = request.GetDependencyScope();
            TService service = (TService)dependencyScope.GetService(typeof(TService));
            return service;
        }

        public static IRepository<T> GetDataRepository<T>(this HttpRequestMessage request) 
            where T  : BaseEntity, new ()
        {
            return request.GetService<IRepository<T>>();
        } 

    }
}