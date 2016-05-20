using Domain.Entities;
using System.Data.Entity;
using System.Linq;

namespace Data.EFData.Extentions
{
    public static class PagingExtention
    {
        /// <summary>
        /// Paging Quqery
        /// </summary>
        /// <typeparam name="T">Type Derived from BaseEntity</typeparam>
        /// <param name="quaery"></param>
        /// <param name="skip">entity to skip</param>
        /// <param name="take">Q-ty results</param>
        /// <returns></returns>
        public static IQueryable<T> Paging<T>(this IQueryable<T> quaery, int skip, int take) where T:BaseEntity
        {
            return quaery.OrderByDescending(x => x.Id)
                .Skip(() => skip)
                .Take(() => take);
        }
    }
}
